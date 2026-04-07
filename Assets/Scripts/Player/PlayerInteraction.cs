using UnityEngine;
using UnityEngine.InputSystem;
using System;
using System.Collections;
using System.Collections.Generic;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] private PlayerAnimations playerAnims;
    private IInteractable currentInteractable;
    private bool interactionStarted = false;

    public static Action<IInteractable> OnInteractableApproached;
    public static Action<IInteractable> OnInteractableLeft;
    public static Action OnIntroSkip;
    public static Action OnPerkActivated;

    [SerializeField] private Inventory playerInventory;

    [Header("Raycast Settings")]
    [SerializeField] private Transform rayOrigin;  
    [SerializeField] private float maxDistance;
    [SerializeField] private LayerMask interactableLayer;    

    private bool isInputBlocked = false; // true means the input is blocked
    private bool introSkipped = false;

    private void OnEnable()
    {
        Inventory.OnObjectReceive += InsertAntibiotics;
        XPCounter.OnLevelUp += BlockInputLevelUp;
    }

    private void OnDisable()
    {
        Inventory.OnObjectReceive -= InsertAntibiotics;
        XPCounter.OnLevelUp -= BlockInputLevelUp;
    }

    private void BlockInputLevelUp()
    {
        BlockInput();
        Invoke("UnblockInput", 1.5f);
    }

    private void BlockInput()
    {
        isInputBlocked = true;
    }

    private void UnblockInput()
    {
        isInputBlocked = false;
    }

    private void Update()
    {
        CheckForInteractable();
    }

    private void CheckForInteractable()
    {
        IInteractable detected = null;

        if (Physics.Raycast(rayOrigin.position, rayOrigin.forward, out RaycastHit hit, maxDistance, interactableLayer))
        {
            hit.collider.TryGetComponent<IInteractable>(out detected);
        }

        if (detected == currentInteractable) return;

        if (currentInteractable != null)
        {
            if (interactionStarted && currentInteractable.IsInteractionPossible())
                PrimaryInteractEnded();

            OnInteractableLeft?.Invoke(currentInteractable);
            currentInteractable = null;
        }

        if (detected != null)
        {
            currentInteractable = detected;
            InteractableApproached();
            OnInteractableApproached?.Invoke(currentInteractable);
        }
    }

    public void OnInteractPrimary(InputAction.CallbackContext context)
    {
        if (isInputBlocked) return;

        if (!introSkipped) // skips intro
        {
            introSkipped = true;
            OnIntroSkip?.Invoke();
            return;
        }

        if (currentInteractable == null)
        {
            return;
        }

        if (!currentInteractable.IsInteractionPossible())
        {            
            return;
        }

        if (context.started)
        {
            PrimaryInteractStarted();
        }

        if (context.canceled && interactionStarted)
        {
            PrimaryInteractEnded();
        }
    }

    private void InteractableApproached()
    {
        switch (currentInteractable)
        {
            case IBench bench:
                switch (bench.GetBenchType())
                {
                    case Bench.BenchType.Armory:
                        UIController.Instance.armoryUI.SetInventory(bench.GetInventory());
                        if (bench is Armory armory)
                        {
                            UIController.Instance.armoryUI.SetArmory(armory);
                        }
                        break;
                    default:
                        UIController.Instance.interactableInvSingleItem.SetInventory(bench.GetInventory());
                        break;
                }
                break;

            case IConstructible constructible:
                break;
        }
    }

    private void PrimaryInteractStarted()
    {
        if (currentInteractable == null) return;
        if (!currentInteractable.IsInteractionPossible()) return;

        interactionStarted = true;
        currentInteractable.StartInteractionPrimary();        
    }

    private void PrimaryInteractEnded()
    {
        interactionStarted = false;
        currentInteractable.EndInteractionPrimary();

        if (playerInventory.GetObjectList().Count == 0)
        {
            playerAnims.SetCarrying(false);
        }
    }

    public void OnInteractSecondary(InputAction.CallbackContext context)
    {
        if (isInputBlocked) return;

        if (PerkController.Instance.isSelectingPerk)
        {
            if (context.started) OnPerkActivated?.Invoke();
            return;
        }

        if (currentInteractable == null)
        {
            return;
        }

        if (!currentInteractable.IsInteractionPossible())
        {
            return;
        }

        if (context.started)
        {
            interactionStarted = true;
            currentInteractable.StartInteractionSecondary();
        }

        if (context.canceled)
        {
            interactionStarted = false;
            currentInteractable.EndInteractionSecondary();
        }
    }

    private void InsertAntibiotics(Object obj, Inventory myInventory) // when antibiotics are found on mission they are automatically added to medical cabinet
    {
        if (myInventory != playerInventory)
        {
            return;
        }

        if (obj is Antibiotics antibiotics)
        {
            ResourceController.Instance.ChangeAntibioticsAmount(antibiotics.currentDurability);
            antibiotics.DestroyObject();
        }
    }
}
