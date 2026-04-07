using UnityEngine;
using System;

public class RepairMasteryHandler : MonoBehaviour
{
    public RepairMastery repairMasterySO;
    private bool isActive = false;

    public static Action OnRepairMasteryProc;

    private void OnEnable()
    {
        RepairMastery.OnRepairMasteryActivated += ActivateRepairMastery;
        Workbench.OnRepairTick += FreeRepairChance;
    }

    private void OnDisable()
    {
        RepairMastery.OnRepairMasteryActivated -= ActivateRepairMastery;
        Workbench.OnRepairTick -= FreeRepairChance;
    }

    private void ActivateRepairMastery()
    {
        isActive = true;
    }

    private void FreeRepairChance()
    {
        if (!isActive) return;

        int roll = UnityEngine.Random.Range(0, 100);

        if (roll <= repairMasterySO.freeRepairChance)
        {
            ResourceController.Instance.ChangeSparePartsAmount(repairMasterySO.sparePartsReturnAmount);
            OnRepairMasteryProc?.Invoke();
        }
    }
}
