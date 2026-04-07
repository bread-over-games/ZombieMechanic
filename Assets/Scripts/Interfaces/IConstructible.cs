public interface IConstructible : IInteractable
{
    int GetSparePartsRequired();
    int GetSparePartsTickCost();
    float GetCurrentConstructionLevel();
    string GetName();
    string GetDescription();
}
