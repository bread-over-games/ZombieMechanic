public interface IConstructible : IInteractable
{
    int GetSparePartsRequired();
    float GetCurrentConstructionLevel();
    string GetName();
    string GetDescription();
}
