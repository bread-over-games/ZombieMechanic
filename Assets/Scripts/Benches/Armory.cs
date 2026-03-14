using UnityEngine;

public class Armory : Bench, IInteractable
{
    public Armor storedArmor;
    public Backpack storedBackpack;
    public Weapon storedWeapon;

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
    }    

    private void OnDisable()
    {
        Inventory.OnObjectReceive -= AssignCurrentObject;
        Inventory.OnObjectSend += RemoveCurrentObject;
    }

    public override void StartInteractionSecondary() // sending on mission
    {
        if (inventory.GetObjectList().Count == 0)
        { 
            return;
        }


        MissionController.Instance.SendMission(storedWeapon, storedBackpack, storedArmor, this);
        
        
        inventory.SendObjectOnMission(storedWeapon);
        inventory.SendObjectOnMission(storedBackpack);
    }

    public override void EndInteractionSecondary()
    {

    }

    private void AssignCurrentObject(Inventory.InventoryOfType invOfType, Object obj) // when putting Object into Armory
    {
        if (invOfType != Inventory.InventoryOfType.Armory)
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

    private void RemoveCurrentObject(Inventory.InventoryOfType invOfType, Object obj) // when taking Object out of Armory
    {
        if (invOfType != Inventory.InventoryOfType.Armory)
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
