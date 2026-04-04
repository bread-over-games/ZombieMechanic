using UnityEngine;
using System;

public class SalvageMasteryHandler : MonoBehaviour
{
    public SalvageMastery salvageMasterySO;
    private bool isActive = false;

    public static Action OnSalvageMasteryProc;

    private void OnEnable()
    {
        SalvageMastery.OnSalvageMasteryActivated += ActivateSalvageMastery;
        SalvageTable.OnSalvageTick += ExtraSalvageChance;
    }

    private void OnDisable()
    {
        SalvageMastery.OnSalvageMasteryActivated -= ActivateSalvageMastery;
        SalvageTable.OnSalvageTick -= ExtraSalvageChance;
    }

    private void ActivateSalvageMastery()
    {
        isActive = true;
    }

    private void ExtraSalvageChance()
    {
        if (!isActive) return;

        int roll = UnityEngine.Random.Range(0, 100);

        if (roll <= salvageMasterySO.extraSalvageChance)
        {
            ResourceController.Instance.ChangeSparePartsAmount(salvageMasterySO.extraSalvageAmount);
            OnSalvageMasteryProc?.Invoke();
        }
    }
}
