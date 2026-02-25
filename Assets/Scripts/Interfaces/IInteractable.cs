using UnityEngine;

public interface IInteractable
{
    void StartInteractionPrimary();
    void EndInteractionPrimary();
    void StartInteractionSecondary();
    void EndInteractionSecondary();
    bool IsInteractionPossible();
}
