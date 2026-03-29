using UnityEngine;
using System;

public class TutorialController : MonoBehaviour
{
    public static TutorialController Instance { get; private set; }

    public EmissionFlasher lootTableFlasher;
    public EmissionFlasher workbenchFlasher;
    public EmissionFlasher salvageTableFlasher;
    public EmissionFlasher medicalCabinetFlasher;
    public EmissionFlasher armoryFlasher;

    public bool skipTutorial = false;
    [HideInInspector] public bool sparePartsPicked = false;
    [HideInInspector] public bool sparePartsPlacedSalvage = false;
    [HideInInspector] public bool sparePartsSalvaged = false;
    [HideInInspector] public bool baseballBatPicked = false;
    [HideInInspector] public bool baseballBatPlacedWorkbench = false;    
    [HideInInspector] public bool baseballBatRepaired = false;
    [HideInInspector] public bool baseballBatPickedWorkbench = false;
    [HideInInspector] public bool baseballBatPlacedArmory = false;
    [HideInInspector] public bool sentOnMissionArmory = false;

    public static Action OnTutorialEnd;
    public static Action OnTutorialStart;
    public static Action<Inventory> OnSpawnTutorialSpareParts;
    public static Action<Inventory> OnSpawnTutorialBaseballBat;

    private void OnEnable()
    {
        LootTable.OnTutorialSparePartsPicked += SparePartsPicked;
        LootTable.OnTutorialSparePartsPicked += StartSalvageTableFlash;
        SalvageTable.OnTutorialSparePartsPlaced += StopSalvageTableFlash;
        SalvageTable.OnTutorialSparePartsSalvaged += SpawnBaseballBat;
        LootTable.OnTutorialBaseballBatPicked += BaseballBatPicked;
        Workbench.OnTutorialBaseballBatPlaced += BaseballBatPlacedWorkbench;
        Workbench.OnTutorialBaseballBatPicked += BaseballBatPickedWorkbench;
        Workbench.OnTutorialBaseballBatRepaired += BaseballBatRepaired;
        Armory.OnBaseballBatPlaced += BaseballBatPlacedArmory;
        Armory.OnSentOnMission += ArmorySentOnMission;
    }

    private void OnDisable()
    {
        LootTable.OnTutorialSparePartsPicked -= SparePartsPicked;
        LootTable.OnTutorialSparePartsPicked -= StartSalvageTableFlash;
        SalvageTable.OnTutorialSparePartsPlaced -= StopSalvageTableFlash;
        SalvageTable.OnTutorialSparePartsSalvaged -= SpawnBaseballBat;
        LootTable.OnTutorialBaseballBatPicked -= BaseballBatPicked;
        Workbench.OnTutorialBaseballBatPlaced -= BaseballBatPlacedWorkbench;
        Workbench.OnTutorialBaseballBatPicked -= BaseballBatPickedWorkbench;
        Workbench.OnTutorialBaseballBatRepaired -= BaseballBatRepaired;
        Armory.OnBaseballBatPlaced -= BaseballBatPlacedArmory;
        Armory.OnSentOnMission -= ArmorySentOnMission;
    }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        if (!skipTutorial)
        {
            OnTutorialStart?.Invoke();
            SpawnSpareParts();
        }
        else
        {
            OnTutorialEnd?.Invoke();
        }
    }    

    private void SpawnSpareParts()
    {
        OnSpawnTutorialSpareParts?.Invoke(InventoriesController.Instance.lootTableInventory);
        lootTableFlasher.StartFlash();
    }

    private void SparePartsPicked()
    {
        lootTableFlasher.StopFlash();
        sparePartsPicked = true;
    }
   
    private void StartSalvageTableFlash()
    {
        salvageTableFlasher.StartFlash();
    }

    private void StopSalvageTableFlash()
    {
        salvageTableFlasher.StopFlash();
        sparePartsPlacedSalvage = true;
    }

    private void SpawnBaseballBat()
    {
        salvageTableFlasher.StopFlash();
        OnSpawnTutorialBaseballBat?.Invoke(InventoriesController.Instance.lootTableInventory);
        sparePartsSalvaged = true;
        lootTableFlasher.StartFlash();
    }

    private void BaseballBatPicked()
    {
        lootTableFlasher.StopFlash();
        workbenchFlasher.StartFlash();
        baseballBatPicked = true;
    }

    private void BaseballBatPlacedWorkbench()
    {
        baseballBatPlacedWorkbench = true;
    }

    private void BaseballBatPickedWorkbench()
    {
        workbenchFlasher.StopFlash();
        baseballBatPickedWorkbench = true;
    }

    private void BaseballBatRepaired()
    {
        armoryFlasher.StartFlash();
        baseballBatRepaired = true; 
    }

    private void BaseballBatPlacedArmory()
    {
        baseballBatPlacedArmory = true;
    }

    private void ArmorySentOnMission()
    {
        sentOnMissionArmory = true;
        armoryFlasher.StopFlash();
        TutorialFinished();
    }

    private void TutorialFinished()
    {
        OnTutorialEnd?.Invoke();
    }
}
