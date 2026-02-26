using UnityEngine;
using UnityEngine.InputSystem;
using System;
using System.Collections;
using System.Collections.Generic;

public class PlayerInteraction : MonoBehaviour
{
    private IInteractable currentInteractable;
    private bool interactionStarted = false;

    public static Action OnInteractableApproached;
    public static Action OnInteractableLeft;

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

        OnInteractableLeft?.Invoke();
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
        UIController.Instance.interactableInvSingleItem.SetInventory(currentInteractable.GetInventory());
        OnInteractableApproached?.Invoke();        
    }

    private void PrimaryInteractStarted()
    {
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
        if (!currentInteractable.IsInteractionPossible())
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
