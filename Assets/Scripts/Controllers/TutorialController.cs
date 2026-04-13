using UnityEngine;
using System;

public class TutorialController : MonoBehaviour
{
    public static TutorialController Instance { get; private set; }

    public EmissionFlasher lootTableFlasher;
    public EmissionFlasher workbenchFlasher;
    public EmissionFlasher salvageTableFlasher;
    public EmissionFlasher[] medicalCabinetFlasher;
    public EmissionFlasher[] armoryFlasher;
    public EmissionFlasher storageRackConstructible;

    public Armory armoryFirst;
    public SalvageTable salvageTable;

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
    private bool medicalCabinetTutorialStarted = false;
    private bool antibioticsUsed = false;
    private bool storageRackBuilt = false;

    public static Action OnTutorialEnd;
    public static Action OnTutorialStart;
    public static Action<Inventory> OnSpawnTutorialSpareParts;
    public static Action<Inventory> OnSpawnTutorialBaseballBat;

    private void OnEnable()
    {
        LootTable.OnTutorialSparePartsPicked += SparePartsPicked;
        LootTable.OnTutorialSparePartsPicked += StartSalvageTableFlash;
        SalvageTable.OnTutorialSparePartsPlaced += SparePartsPlacedSalvage;
        SalvageTable.OnTutorialSparePartsSalvaged += SpawnBaseballBat;
        LootTable.OnTutorialBaseballBatPicked += BaseballBatPicked;
        Workbench.OnTutorialBaseballBatPlaced += BaseballBatPlacedWorkbench;
        Workbench.OnTutorialBaseballBatPicked += BaseballBatPickedWorkbench;
        Workbench.OnTutorialBaseballBatRepaired += BaseballBatRepaired;
        Armory.OnBaseballBatPlaced += BaseballBatPlacedArmory;
        Armory.OnSentOnMission += ArmorySentOnMission;
        Infection.OnInfectionLevelChange += InfectionTutorialStart;
        MedicalCabinet.OnAntibioticsUsed += InfectionTutorialStop;
        ResourceController.OnSparePartsLimitReached += BuildStorageRackFlashStart;        
        StorageRack.OnStorageRackBuilt += BuildStorageRackFlashStop;
    }

    private void OnDisable()
    {
        LootTable.OnTutorialSparePartsPicked -= SparePartsPicked;
        LootTable.OnTutorialSparePartsPicked -= StartSalvageTableFlash;
        SalvageTable.OnTutorialSparePartsPlaced -= SparePartsPlacedSalvage;
        SalvageTable.OnTutorialSparePartsSalvaged -= SpawnBaseballBat;
        LootTable.OnTutorialBaseballBatPicked -= BaseballBatPicked;
        Workbench.OnTutorialBaseballBatPlaced -= BaseballBatPlacedWorkbench;
        Workbench.OnTutorialBaseballBatPicked -= BaseballBatPickedWorkbench;
        Workbench.OnTutorialBaseballBatRepaired -= BaseballBatRepaired;
        Armory.OnBaseballBatPlaced -= BaseballBatPlacedArmory;
        Armory.OnSentOnMission -= ArmorySentOnMission;
        Infection.OnInfectionLevelChange -= InfectionTutorialStart;
        MedicalCabinet.OnAntibioticsUsed -= InfectionTutorialStop;
        ResourceController.OnSparePartsLimitReached -= BuildStorageRackFlashStart;
        StorageRack.OnStorageRackBuilt -= BuildStorageRackFlashStop;
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
            DisableArmories();
        }
        else
        {
            OnTutorialEnd?.Invoke();
        }
    }  
    
    private void BuildStorageRackFlashStart()
    {
        if (!storageRackBuilt)
        {
            storageRackConstructible.StartFlash();
            storageRackBuilt = true;
        }        
    }

    private void BuildStorageRackFlashStop(int value)
    {
        storageRackConstructible.StopFlash();
    }

    private void InfectionTutorialStart(float infectionLevel)
    {
        if (infectionLevel > 60 && !medicalCabinetTutorialStarted && !skipTutorial)
        {
            foreach (EmissionFlasher medCabFlash in medicalCabinetFlasher)
            {
                medCabFlash.StartFlash();
            }            
            medicalCabinetTutorialStarted = true;
        }
    }

    private void InfectionTutorialStop()
    {
        if (medicalCabinetTutorialStarted && !antibioticsUsed)
        {
            foreach (EmissionFlasher medCabFlash in medicalCabinetFlasher)
            {
                medCabFlash.StopFlash();
            }
            antibioticsUsed = true;
        }
    }

    private void DisableArmories()
    {
        armoryFirst.isEnabled = false;
    }

    private void EnableArmories()
    {
        armoryFirst.isEnabled = true;
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

    private void SparePartsPlacedSalvage()
    {
        sparePartsPlacedSalvage = true;
    }

    private void SpawnBaseballBat()
    {
        salvageTableFlasher.StopFlash();
        OnSpawnTutorialBaseballBat?.Invoke(InventoriesController.Instance.lootTableInventory);
        sparePartsSalvaged = true;
        lootTableFlasher.StartFlash();
        salvageTable.DisableSalvageTable();
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
        foreach (EmissionFlasher armFlasher in armoryFlasher)
        {
            armFlasher.StartFlash();
        }

        baseballBatRepaired = true;
        EnableArmories();
    }

    private void BaseballBatPlacedArmory()
    {
        baseballBatPlacedArmory = true;
    }

    private void ArmorySentOnMission()
    {
        sentOnMissionArmory = true;
        foreach (EmissionFlasher armFlasher in armoryFlasher)
        {
            armFlasher.StopFlash();
        }
        TutorialFinished();
        salvageTable.EnableSalvageTable();
    }

    private void TutorialFinished()
    {
        OnTutorialEnd?.Invoke();
    }
}
