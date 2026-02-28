/// Displays or destroys given object in the world

using UnityEngine;

public class ObjectDisplay : MonoBehaviour
{
    private GameObject currentObject;
    [SerializeField] private Inventory inventory;
    [SerializeField] private Transform weaponSpawnPivot;

    private void OnEnable()
    {
        Inventory.OnObjectReceive += DisplayCurrentObject;
        Inventory.OnObjectSend += DestroyCurrentWeapon;
    }

    public void StartInteractionPrimary()
    {
        Inventory.OnObjectReceive -= DisplayCurrentObject;
        Inventory.OnObjectSend -= DestroyCurrentWeapon;
    }

    private void DisplayCurrentObject(Inventory.InventoryOfType inventoryOfType, Object obj)
    {
        if (inventoryOfType == inventory.GetInventoryOfType() && inventory.GetObjectList().Count > 0)
        {
            switch (inventory.GetObjectList()[0])
            {
                case Weapon weapon:
                    currentObject = WeaponWorld.SpawnWeaponWorld(weaponSpawnPivot.position, weapon, weaponSpawnPivot);
                    break;
                    /*case Scrap scrap:
                        scrap.LoadValues(scrap);
                        break;
                    case Medicine medicine:
                        medicine.LoadValues(medicine);
                        break;*/
            }            
        }
    }

    private void DestroyCurrentWeapon()
    {
        if (inventory.GetObjectList().Count == 0 && currentObject != null)
        {
            Destroy(currentObject);
        }
    }
}
