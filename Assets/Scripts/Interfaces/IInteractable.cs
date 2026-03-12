using UnityEngine;

public interface IInteractable
{
    Bench.BenchType GetBenchType();
    void StartInteractionPrimary();
    void EndInteractionPrimary();
    void StartInteractionSecondary();
    void EndInteractionSecondary();
    bool IsInteractionPossible();
    Inventory GetInventory();
    string GetName();

    bool CanAcceptObject(Object objectToPlace);
}
