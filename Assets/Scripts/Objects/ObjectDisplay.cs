/// Displays or destroys given object in the world

using UnityEngine;
using static UnityEditor.Progress;

public class ObjectDisplay : MonoBehaviour
{
    private GameObject currentObject;
    [SerializeField] private Inventory inventory;
    [SerializeField] private Transform weaponSpawnPivot;

    private void OnEnable()
    {
        Inventory.OnObjectReceive += DisplayCurrentObject;
        Inventory.OnObjectSend += TryDisplayNextObject;
    }

    public void StartInteractionPrimary()
    {
        Inventory.OnObjectReceive -= DisplayCurrentObject;
        Inventory.OnObjectSend -= TryDisplayNextObject;
    }

    private void DisplayCurrentObject(Inventory.InventoryOfType inventoryOfType, Object obj)
    {
        DestroyCurrentObject();
        if (inventoryOfType == inventory.GetInventoryOfType() && inventory.GetObjectList().Count > 0)
        {
            switch (inventory.GetObjectList()[0])
            {
                case Weapon weapon:
                    currentObject = WeaponWorld.SpawnWeaponWorld(weaponSpawnPivot.position, weapon, weaponSpawnPivot);
                    break;
                case Scrap scrap:
                    currentObject = ScrapWorld.SpawnScrapWorld(weaponSpawnPivot.position, scrap, weaponSpawnPivot);
                    break;
                /*    case Medicine medicine:
                        medicine.LoadValues(medicine);
                        break;*/
            }
        }
    }

    private void TryDisplayNextObject()
    {
        DestroyCurrentObject();

        if (inventory.GetObjectList().Count > 0)
        {
            switch (inventory.GetObjectList()[0])
            {
                case Weapon weapon:
                    currentObject = WeaponWorld.SpawnWeaponWorld(weaponSpawnPivot.position, weapon, weaponSpawnPivot);
                    break;
                case Scrap scrap:
                    currentObject = ScrapWorld.SpawnScrapWorld(weaponSpawnPivot.position, scrap, weaponSpawnPivot);
                    break;
                    /*    case Medicine medicine:
                            medicine.LoadValues(medicine);
                            break;*/
            }
        }
    }

    private void DestroyCurrentObject()
    {
        if (inventory.GetObjectList().Count >= 0 && currentObject != null)
        {            
            Destroy(currentObject);
            currentObject = null;
        }
    }
}
