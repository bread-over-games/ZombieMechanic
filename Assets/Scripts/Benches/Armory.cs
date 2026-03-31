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

    private ButtonSelector.ArmorySlot currentSlotSelection;

    public static Action OnBaseballBatPlaced;
    public static Action OnSentOnMission;

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
        UIArmory.OnCurrentArmorySlotSelected += AssignCurrentSlotSelection;        
    }    

    private void OnDisable()
    {
        Inventory.OnObjectReceive -= AssignCurrentObject;
        Inventory.OnObjectSend -= RemoveCurrentObject;
        UIArmory.OnCurrentArmorySlotSelected -= AssignCurrentSlotSelection;
    }

    public void MakeArmoryAvailableForMission()
    {
        isAvailableForMission = true; 
    }

    private void AssignCurrentSlotSelection(ButtonSelector.ArmorySlot selectedSlot)
    {
        currentSlotSelection = selectedSlot;
    }

    public override bool IsInteractionPossible()
    {
        if (!isEnabled)
        {  
            return false; 
        }

        return isAvailableForMission;
    }

    public override void StartInteractionPrimary()
    {
        if (!TutorialController.Instance.skipTutorial)
        {
            if (!TutorialController.Instance.baseballBatPlacedArmory)
            {
                OnBaseballBatPlaced?.Invoke();
                return;
            }
        }

        if (InventoriesController.Instance.playerInventory.GetObjectList().Count == 0)
        {
            if (inventory.GetObjectList().Count > 0)
            {
                switch (currentSlotSelection)
                {
                    case ButtonSelector.ArmorySlot.Weapon: 
                        if (storedWeapon != null)
                            inventory.SendObject(InventoriesController.Instance.playerInventory, storedWeapon);                        
                        break;
                    case ButtonSelector.ArmorySlot.Armor:
                        if (storedArmor != null)
                            inventory.SendObject(InventoriesController.Instance.playerInventory, storedArmor);
                        break;
                    case ButtonSelector.ArmorySlot.Backpack:
                        if (storedBackpack != null)
                            inventory.SendObject(InventoriesController.Instance.playerInventory, storedBackpack);                        
                        break;
                }                
            }
        }
    }

    public override void StartInteractionSecondary() // sending on mission
    {
        if (!isAvailableForMission)
        {
            return;
        }

        isAvailableForMission = false;

        MissionController.Instance.SendMission(storedWeapon, storedBackpack, storedArmor, inventory, this);

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

    public override void EndInteractionSecondary()
    {

    }

    private void AssignCurrentObject(Object obj, Inventory myInventory) // when putting Object into Armory
    {        
        if (myInventory != inventory)
        {
            return;
        }

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
        if (inventory != myInventory)
        {
            return;
        }

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
