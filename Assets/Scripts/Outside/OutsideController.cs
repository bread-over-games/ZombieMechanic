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
        for (int i = inventory.GetWeaponList().Count - 1; i >= 0; i--)
        {
            float howLongOutside = Time.time - inventory.GetWeaponList()[i].timeAddedToOutside;
            if (howLongOutside > inventory.GetWeaponList()[i].timeToSpendOutside)
            {
                ReturnWeaponFromOutside();
            }
        }
    }

    private void ReturnWeaponFromOutside()
    {
        if (!inventory.GetWeaponList()[0].DecayWeapon(inventory)) // decays weapon, checks if it is destroyed or not
        {
            inventory.SendWeapon(InventoriesController.Instance.lootTableInventory);
            OnSurvivorReturned?.Invoke();
        }
    }    
}
