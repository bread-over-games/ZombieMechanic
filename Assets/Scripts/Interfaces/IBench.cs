public interface IBench : IInteractable
{
    Bench.BenchType GetBenchType();
    Inventory GetInventory();
    string GetName();
    bool CanAcceptObject(Object objectToPlace);
}
