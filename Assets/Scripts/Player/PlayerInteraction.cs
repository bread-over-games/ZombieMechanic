using UnityEngine;
using UnityEngine.InputSystem;
using System;
using System.Collections;
using System.Collections.Generic;

public class PlayerInteraction : MonoBehaviour
{
    private IInteractable currentInteractable;
    private bool interactionStarted = false;

    public static Action<Bench.BenchType> OnInteractableApproached;
    public static Action<Bench.BenchType> OnInteractableLeft;

    [SerializeField] private Inventory playerInventory;

    private void OnTriggerEnter(Collider other)
    {        
        if (other.TryGetComponent<IInteractable>(out IInteractable interactable))
        {            
            currentInteractable = interactable;
            InteractableApproached();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (currentInteractable != null && interactionStarted && currentInteractable.IsInteractionPossible())
        {                
            PrimaryInteractEnded();            
        }

        OnInteractableLeft?.Invoke(currentInteractable.GetBenchType());
        currentInteractable = null;
    }

    public void OnInteractPrimary(InputAction.CallbackContext context)
    {
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
                break;
            default:
                UIController.Instance.interactableInvSingleItem.SetInventory(currentInteractable.GetInventory());
                break;
        }
        
        OnInteractableApproached?.Invoke(currentInteractable.GetBenchType());        
    }

    private void PrimaryInteractStarted()
    {
        Inventory currentInventory = currentInteractable.GetInventory();
        if (playerInventory.GetObjectList().Count > 0 && currentInventory.GetObjectList().Count < currentInventory.GetCapacity()) // there is a space in the target inventory
        {
            if (currentInteractable.CanAcceptObject(playerInventory.GetObjectList()[0]))
            {
                playerInventory.SendObject(currentInteractable.GetInventory()); // deposit item
                return;
            }
        }

        interactionStarted = true;
        currentInteractable.StartInteractionPrimary();
    }

    private void PrimaryInteractEnded()
    {
        interactionStarted = false;
        currentInteractable.EndInteractionPrimary();
    }

    public void OnInteractSecondary(InputAction.CallbackContext context)
    {
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
}
