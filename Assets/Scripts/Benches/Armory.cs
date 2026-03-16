using UnityEngine;
using UnityEngine.EventSystems;

public class Armory : Bench, IInteractable
{
    [SerializeReference] public Armor storedArmor = null;
    [SerializeReference] public Backpack storedBackpack = null;
    [SerializeReference] public Weapon storedWeapon = null;

    private ButtonSelector.ArmorySlot currentSlotSelection;

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

    private void AssignCurrentSlotSelection(ButtonSelector.ArmorySlot selectedSlot)
    {
        currentSlotSelection = selectedSlot;
    }

    public override void StartInteractionPrimary()
    {
        if (InventoriesController.Instance.playerInventory.GetObjectList().Count == 0)
        {
            if (inventory.GetObjectList().Count > 0)
            {
                switch (currentSlotSelection)
                {
                    case ButtonSelector.ArmorySlot.Weapon:                        
                        inventory.SendObject(InventoriesController.Instance.playerInventory, storedWeapon);                        
                        break;
                    case ButtonSelector.ArmorySlot.Armor:
                        inventory.SendObject(InventoriesController.Instance.playerInventory, storedArmor);
                        break;
                    case ButtonSelector.ArmorySlot.Backpack:                       
                        inventory.SendObject(InventoriesController.Instance.playerInventory, storedBackpack);                        
                        break;
                }                
            }
        }
    }

    public override void StartInteractionSecondary() // sending on mission
    {
        if (inventory.GetObjectList().Count == 0)
        { 
            return;
        }

        MissionController.Instance.SendMission(storedWeapon, storedBackpack, storedArmor, inventory);

        inventory.SendObjectOnMission(storedArmor);
        inventory.SendObjectOnMission(storedWeapon);
        inventory.SendObjectOnMission(storedBackpack);
        Debug.Log("Sent on mission!");
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
