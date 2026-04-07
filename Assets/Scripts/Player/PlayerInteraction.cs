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

    public static Action<Bench.BenchType> OnInteractableApproached;
    public static Action<Bench.BenchType> OnInteractableLeft;
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

            OnInteractableLeft?.Invoke(currentInteractable.GetBenchType());
            currentInteractable = null;
        }

        if (detected != null)
        {
            currentInteractable = detected;
            InteractableApproached();
            OnInteractableApproached?.Invoke(currentInteractable.GetBenchType());
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
        switch (currentInteractable.GetBenchType())
        {
            case Bench.BenchType.Armory:
                UIController.Instance.armoryUI.SetInventory(currentInteractable.GetInventory());

                if (currentInteractable is Armory armory) 
                {
                    UIController.Instance.armoryUI.SetArmory(armory);
                }
               
                break;
            default:
                UIController.Instance.interactableInvSingleItem.SetInventory(currentInteractable.GetInventory());
                break;
        }        
    }

    private void PrimaryInteractStarted()
    {
        Inventory currentInventory = currentInteractable.GetInventory();

        if (playerInventory.GetObjectList().Count == 0) // player has no item in hands and wants to pick up from bench
        {
            interactionStarted = true;
            currentInteractable.StartInteractionPrimary();
            playerAnims.SetCarrying(true);
        } else         
        if (currentInventory.GetObjectList().Count < currentInventory.GetCapacity()) // player has item and there is a space in the target inventory
        {
            if (currentInteractable.CanAcceptObject(playerInventory.GetObjectList()[0])) // interactable can accept object of given type
            {
                currentInteractable.StartInteractionPrimary();
                playerInventory.SendObject(currentInteractable.GetInventory(), playerInventory.GetObjectList()[0]); // deposit item
                playerAnims.SetCarrying(false);
            }
        }     
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
