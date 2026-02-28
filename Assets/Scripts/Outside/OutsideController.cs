using UnityEngine;
using System;

public class OutsideController : MonoBehaviour
{
    [SerializeField] private Inventory inventory;
    float timer = 0f;

    public static Action OnSurvivorReturned;

    private void Update()
    {
        CheckWeaponOutsideTimer();
    }

    private void CheckWeaponOutsideTimer() // checks how long a weapon has been outside
    {
        for (int i = inventory.GetObjectList().Count - 1; i >= 0; i--)
        {
            if (inventory.GetObjectList()[i] is Weapon weapon)
            {
                float howLongOutside = Time.time - weapon.timeAddedToOutside;
                if (howLongOutside > weapon.timeToSpendOutside)
                {
                    ReturnWeaponFromOutside();
                }
            }
        }
    }

    private void ReturnWeaponFromOutside()
    {
        if (inventory.GetObjectList()[0] is Weapon weapon)
        {
            if (!weapon.DecayWeapon(inventory)) // decays weapon, checks if it is destroyed or not
            {
                inventory.SendWeapon(InventoriesController.Instance.lootTableInventory);
                OnSurvivorReturned?.Invoke();
            }
        }
    }    
}
