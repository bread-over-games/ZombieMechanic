using UnityEngine;

public class SalvageMasteryHandler : MonoBehaviour
{
    public SalvageMastery salvageMasterySO;
    private bool isActive = false;

    private void OnEnable()
    {
        SalvageMastery.OnSalvageMasteryActivated += ActivateSalvageMastery;
        SalvageTable.OnSalvage += ExtraSalvageChance;
    }

    private void OnDisable()
    {
        SalvageMastery.OnSalvageMasteryActivated -= ActivateSalvageMastery;
        SalvageTable.OnSalvage -= ExtraSalvageChance;
    }

    private void ActivateSalvageMastery()
    {
        isActive = true;
    }

    private void ExtraSalvageChance()
    {
        if (!isActive) return;

        int roll = Random.Range(0, 100);

        if (roll <= salvageMasterySO.extraSalvageChance)
        {
            ResourceController.Instance.ChangeSparePartsAmount(salvageMasterySO.extraSalvageAmount);            
        }
    }
}
