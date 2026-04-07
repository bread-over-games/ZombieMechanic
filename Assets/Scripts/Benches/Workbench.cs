using UnityEngine;
using System;
using System.Collections;
public class Workbench : Bench, IBench
{
    [SerializeField] private float repairInterval;
    [SerializeField] private int repairSparePartsCost;
    [SerializeField] private int repairValue;    

    private Coroutine repairCoroutine;

    public static Action OnRepairStart;
    public static Action OnRepairTick;
    public static Action OnRepairStop;

    public static Action OnTutorialBaseballBatPlaced;
    public static Action OnTutorialBaseballBatRepaired;
    public static Action OnTutorialBaseballBatPicked;

    public void Awake()
    {
        acceptedTypes.Add(typeof(Weapon));
        acceptedTypes.Add(typeof(Backpack));
        acceptedTypes.Add(typeof(Armor));
    }

    private void OnEnable()
    {
        Inventory.OnObjectReceive += AssignCurrentObject;
    }

    private void OnDisable()
    {
        Inventory.OnObjectReceive -= AssignCurrentObject;
    }

    private void AssignCurrentObject(Object obj, Inventory myInventory)
    {
        if (myInventory == inventory)
        {
                currentObject = obj;
        }           
    }

    private void TryRepair()
    {        
        if (ResourceController.Instance.CanRepair(repairSparePartsCost))
        {
            if (currentObject.currentDurability < currentObject.maxDurability && currentObject != null)
            {
                repairCoroutine = StartCoroutine(DoRepair());
                OnRepairStart?.Invoke();
            }            
        }
    }

    IEnumerator DoRepair()
    {
        while (currentObject.currentDurability < currentObject.maxDurability)
        {
            yield return new WaitForSeconds(repairInterval);
            ResourceController.Instance.ChangeSparePartsAmount(-repairSparePartsCost);
            currentObject.RepairObject(repairValue);
            OnRepairTick?.Invoke();

            if (currentObject.currentDurability == currentObject.maxDurability || ResourceController.Instance.GetSparePartsAmount() < repairSparePartsCost)
            {
                EndInteractionSecondary();
            }

            if (!ResourceController.Instance.CanRepair(repairSparePartsCost))
            {
                if (!TutorialController.Instance.skipTutorial)
                {
                    if (!TutorialController.Instance.baseballBatRepaired)
                    {
                        OnTutorialBaseballBatRepaired?.Invoke();
                    }                    
                }
                break;
            }
        }        
    }

    public override void StartInteractionPrimary()
    {
        base.StartInteractionPrimary();

        if (!TutorialController.Instance.skipTutorial)
        {
            if (!TutorialController.Instance.baseballBatPlacedWorkbench)
            {
                OnTutorialBaseballBatPlaced?.Invoke();
                return;
            }
            
            if (!TutorialController.Instance.baseballBatPickedWorkbench)
            {
                OnTutorialBaseballBatPicked?.Invoke();
            }
        }
    }

    public override void StartInteractionSecondary()
    {
        TryRepair();
    }

    public override void EndInteractionSecondary()
    {
        if (repairCoroutine != null)
        {
            StopCoroutine(repairCoroutine);
            OnRepairStop?.Invoke(); 
            repairCoroutine = null;
        }
    }
}
