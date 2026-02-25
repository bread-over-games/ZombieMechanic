using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    private IInteractable currentInteractable;
    private bool interactionStarted = false;

    private void OnTriggerEnter(Collider other)
    {        
        if (other.TryGetComponent<IInteractable>(out var interactable))
        {            
            currentInteractable = interactable;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (currentInteractable != null && interactionStarted && currentInteractable.IsInteractionPossible())
        {                
            PrimaryInteractEnded();
        }

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
