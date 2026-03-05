using UnityEngine;

public class Armory : Bench, IInteractable
{
    public Weapon storedWeapon;

    public void Awake()
    {
        acceptedTypes.Add(typeof(Weapon));
    }

    private void OnEnable()
    {
        Inventory.OnObjectReceive += AssignCurrentWeapon;
    }

    private void OnDisable()
    {
        Inventory.OnObjectReceive -= AssignCurrentWeapon;
    }

    public override void StartInteractionSecondary() // sending on mission
    {
        if (inventory.GetObjectList().Count == 0)
        { 
            return;
        }
        
        MissionController.Instance.SendMission(storedWeapon, this);
        inventory.SendObjectOnMission();
    }

    public override void EndInteractionSecondary()
    {

    }

    private void AssignCurrentWeapon(Inventory.InventoryOfType invOfType, Object obj)
    {
        if (invOfType == Inventory.InventoryOfType.Armory && obj is Weapon weapon)
        {
            storedWeapon = weapon;
        }
    }
}
