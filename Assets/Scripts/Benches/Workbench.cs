using UnityEngine;
using System;
using System.Collections;
public class Workbench : Bench, IInteractable
{
    [SerializeField] private float repairInterval;
    [SerializeField] private int repairSalvageCost;
    [SerializeField] private int repairValue;    

    private Coroutine repairCoroutine;

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
        if (ResourceController.Instance.CanRepair(repairSalvageCost))
        {
            if (currentObject.currentDurability < currentObject.maxDurability)
            {
                repairCoroutine = StartCoroutine(DoRepair());
            }            
        }
    }

    IEnumerator DoRepair()
    {
        while (currentObject.currentDurability < currentObject.maxDurability)
        {
            yield return new WaitForSeconds(repairInterval);
            ResourceController.Instance.ChangeSparePartsAmount(-repairSalvageCost);
            currentObject.RepairObject(repairValue);

            if (!ResourceController.Instance.CanRepair(repairSalvageCost))
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
            repairCoroutine = null;
        }
    }
}
