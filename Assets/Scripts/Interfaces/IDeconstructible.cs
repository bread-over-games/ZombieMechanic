public interface IDeconstructible : IInteractable
{
    int GetSparePartsReward();
    int GetSparePartsLeft();
    float GetCurrentDurabilityLevel();
    string GetName();
    string GetDescription();
}
