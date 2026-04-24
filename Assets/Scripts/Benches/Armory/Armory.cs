using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class Armory : Bench, IInteractable
{
    [HideInInspector] public bool isEnabled = true;
    [HideInInspector] public bool isAvailableForMission = true;

    [SerializeReference] public Armor storedArmor = null;
    [SerializeReference] public Backpack storedBackpack = null;
    [SerializeReference] public Weapon storedWeapon = null;

    private UIGearOverview.ArmorySlot currentSlotSelection;

    public static Action OnBaseballBatPlaced;
    public static Action OnSentOnMission; // for tutorial purpose only
    public static Action OnMissionGearSelected;

    public GameObject survivorPrefab; // used for spawning when Armory is complete
    public Transform survivorSpawnSpot;

    public void Awake()
    {
        acceptedTypes.Add(typeof(Weapon));
        acceptedTypes.Add(typeof(Armor));
        acceptedTypes.Add(typeof(Backpack));
    }

    private void OnEnable()
    {
        Inventory.OnObjectReceive += AssignCurrentObject;
        Inventory.OnObjectSend += RemoveCurrentObject;
        UIGearOverview.OnCurrentArmorySlotSelected += AssignCurrentSlotSelection;
        MissionController.OnMissionStarting += SendGearOnMission;
    }    

    private void OnDisable()
    {
        Inventory.OnObjectReceive -= AssignCurrentObject;
        Inventory.OnObjectSend -= RemoveCurrentObject;
        UIGearOverview.OnCurrentArmorySlotSelected -= AssignCurrentSlotSelection;
        MissionController.OnMissionStarting += SendGearOnMission;
    }

    public void SetSurvivorSpawnLocation(Transform pos)
    {
        survivorSpawnSpot = pos;
        SpawnSurvivor();
    }

    private void SpawnSurvivor()
    {
        GameObject spawnedSurvivor = Instantiate(survivorPrefab, survivorSpawnSpot.position, survivorSpawnSpot.rotation, GameObject.Find("Survivors").transform);
        spawnedSurvivor.GetComponent<SurvivorVisualController>().assignedArmory = this;
    }

    public void MakeArmoryAvailableForMission()
    {
        isAvailableForMission = true; 
    }

    private void AssignCurrentSlotSelection(UIGearOverview.ArmorySlot selectedSlot)
    {
        currentSlotSelection = selectedSlot;
    }

    public override bool IsInteractionPossible()
    {
        if (!isEnabled) return false;

        return isAvailableForMission;
    }

    public override void StartInteractionPrimary()
    {
        if (!TutorialController.Instance.skipTutorial)
        {
            if (!TutorialController.Instance.baseballBatPlacedArmory)
            {
                OnBaseballBatPlaced?.Invoke();
            }
        }

        Inventory playerInventory = InventoriesController.Instance.playerInventory;

        if (playerInventory.GetObjectList().Count == 0) // player has no item - try take item from slot
        {
            if (inventory.GetObjectList().Count > 0) // check if there are items in inventory
            {
                switch (currentSlotSelection)
                {
                    case UIGearOverview.ArmorySlot.Weapon: 
                        if (storedWeapon != null)
                            inventory.SendObject(playerInventory, storedWeapon);                        
                        break;
                    case UIGearOverview.ArmorySlot.Armor:
                        if (storedArmor != null)
                            inventory.SendObject(playerInventory, storedArmor);
                        break;
                    case UIGearOverview.ArmorySlot.Backpack:
                        if (storedBackpack != null)
                            inventory.SendObject(playerInventory, storedBackpack);                        
                        break;
                }
                OnObjectPicked?.Invoke(); // player picked item on bench - subscribe animation SetCarrying(true)
            }
        }
        else
        {
            if (inventory.GetObjectList().Count >= inventory.GetCapacity()) return; // bench full
            if (!CanAcceptObject(playerInventory.GetObjectList()[0])) return;

            Object obj = playerInventory.GetObjectList()[0];
            if (obj is Weapon && storedWeapon != null) return;
            if (obj is Armor && storedArmor != null) return;
            if (obj is Backpack && storedBackpack != null) return;

            playerInventory.SendObject(inventory, playerInventory.GetObjectList()[0]);
            OnObjectDeposited?.Invoke(); 
        }
    }

    public override void StartInteractionSecondary() // sending on mission
    {
        if (!isAvailableForMission) return;
        if (storedWeapon == null) return; // cannot send on mission without weapon

        MissionController.Instance.ConfirmMissionGear(storedWeapon, storedBackpack, storedArmor, inventory, this);
        isAvailableForMission = false;   
        OnMissionGearSelected?.Invoke();
    }

    public override void EndInteractionSecondary()
    {

    }

    private void SendGearOnMission(Armory missionStartingArmory)
    {
        if (missionStartingArmory != this) return;        

        inventory.SendObjectOnMission(storedArmor);
        inventory.SendObjectOnMission(storedWeapon);
        inventory.SendObjectOnMission(storedBackpack);

        if (!TutorialController.Instance.skipTutorial)
        {
            if (!TutorialController.Instance.sentOnMissionArmory)
            {
                OnSentOnMission?.Invoke();
            }
        }
    }

    private void AssignCurrentObject(Object obj, Inventory myInventory) // when putting Object into Armory
    {        
        if (myInventory != inventory) return;

        if (obj is Weapon weapon)
        {
            storedWeapon = weapon;
        }

        if (obj is Armor armor)
        {
            storedArmor = armor;    
        }

        if (obj is Backpack backpack)
        {
            storedBackpack = backpack;
        }
    }

    private void RemoveCurrentObject(Object obj, Inventory myInventory) // when taking Object out of Armory
    {
        if (inventory != myInventory) return;


        if (obj is Weapon weapon)
        {
            storedWeapon = null;
        }

        if (obj is Armor armor)
        {
            storedArmor = null;
        }

        if (obj is Backpack backpack)
        {
            storedBackpack = null;
        }
    }
}
