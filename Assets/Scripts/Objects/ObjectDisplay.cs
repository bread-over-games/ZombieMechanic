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
        Inventory.OnObjectSend += DestroyCurrentObject;
        Scrap.OnScrapDestroyed += DestroyMyObject;
    }

    public void StartInteractionPrimary()
    {
        Inventory.OnObjectReceive -= DisplayCurrentObject;
        Inventory.OnObjectSend -= DestroyCurrentObject;
        Scrap.OnScrapDestroyed -= DestroyMyObject;
    }

    private void DisplayCurrentObject(Inventory.InventoryOfType inventoryOfType, Object obj)
    {
        if (inventoryOfType != inventory.GetInventoryOfType())
        {
            return;
        }

        if (inventory.GetObjectList().Count > 0 && currentObject == null)
        {
            DoObjectDisplay();
        }
    }

    private void DoObjectDisplay()
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

    public void DestroyCurrentObject(Inventory.InventoryOfType invOfType)
    {
        if (inventory.GetInventoryOfType() != invOfType)
        { 
            return;
        }

        if (inventory.GetObjectList().Count >= 0 && currentObject != null)
        {            
            Destroy(currentObject);
            currentObject = null;  
            
            if (inventory.GetObjectList().Count > 0)
            {
                DoObjectDisplay();
            }
        }        
    }

    private void DestroyMyObject(Inventory invOfType) // determines if it should destroy object on this interactable
    {
        if (inventory.GetInventoryOfType() == invOfType.GetInventoryOfType())
        {
            DestroyCurrentObject(invOfType.GetInventoryOfType());
        }
    }
}
