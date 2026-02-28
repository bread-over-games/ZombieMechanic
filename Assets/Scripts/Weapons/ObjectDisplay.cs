/// Displays given object in the world

using UnityEngine;

public class ObjectDisplay : MonoBehaviour
{
    private GameObject currentWeapon;
    [SerializeField] private Inventory inventory;
    [SerializeField] private Transform weaponSpawnPivot;

    private void OnEnable()
    {
        Inventory.OnWeaponReceive += DisplayCurrentWeapon;
        Inventory.OnWeaponSend += DestroyCurrentWeapon;
    }

    public void StartInteractionPrimary()
    {
        Inventory.OnWeaponReceive -= DisplayCurrentWeapon;
        Inventory.OnWeaponSend -= DestroyCurrentWeapon;
    }

    private void DisplayCurrentWeapon(Inventory.InventoryOfType inventoryOfType)
    {
        if (inventoryOfType == inventory.GetInventoryOfType() && inventory.GetWeaponList().Count > 0)
        {
            currentWeapon = WeaponWorld.SpawnWeaponWorld(weaponSpawnPivot.position, inventory.GetWeaponList()[0], weaponSpawnPivot);
        }
    }

    private void DestroyCurrentWeapon()
    {
        if (inventory.GetWeaponList().Count == 0 && currentWeapon != null)
        {
            Destroy(currentWeapon);
        }
    }
}
