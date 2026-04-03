using UnityEngine;

public class RepairMasteryHandler : MonoBehaviour
{
    public RepairMastery repairMasterySO;
    private bool isActive = false;

    private void OnEnable()
    {
        RepairMastery.OnRepairMasteryActivated += ActivateRepairMastery;
        Workbench.OnRepair += FreeRepairChance;
    }

    private void OnDisable()
    {
        RepairMastery.OnRepairMasteryActivated -= ActivateRepairMastery;
        Workbench.OnRepair -= FreeRepairChance;
    }

    private void ActivateRepairMastery()
    {
        isActive = true;
    }

    private void FreeRepairChance()
    {
        if (!isActive) return;

        int roll = Random.Range(0, 100);

        if (roll <= repairMasterySO.freeRepairChance)
        {
            ResourceController.Instance.ChangeSparePartsAmount(repairMasterySO.sparePartsReturnAmount);
            Debug.Log("free repair");
        }
    }
}
