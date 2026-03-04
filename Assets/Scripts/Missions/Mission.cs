using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Mission
{
    public Survivor survivor; // current survivor on the mission    
    public bool isComplete = false;
    public Weapon equippedWeapon;
    //public Armory equippedArmor;
    //public Backpack equippedBackpack;

    public float missionDuration;
    public float elapsedTime;

    public Mission(/*Survivor missionSurvivor, */Weapon weaponToEquip)
    {
        // survivor = missionSurvivor;
        equippedWeapon = weaponToEquip;
        elapsedTime = 0f;

        missionDuration = 10f; // for now, no calculation

        Debug.Log("Mission created with weapon " + equippedWeapon.weaponName);
    }

    public void ResolveMission()
    {
        ApplyWearToLoadout();
        ReturnSurvivorLoadout();
        isComplete = true;
        //Debug.Log(survivor.survivorName + " has returned from the mission!");
    }

    private void ReturnSurvivorLoadout()
    {
        InventoriesController.Instance.armoryInventory.ReceiveObject(equippedWeapon);
    }


    private void ApplyWearToLoadout()
    {
       /* {
            if (inventory.GetObjectList()[0] is Weapon weapon)
            {
                if (!weapon.DamageWeapon(inventory, weaponDecayAmount)) // decays weapon, checks if it is destroyed or not
                {
                    inventory.SendObject(InventoriesController.Instance.lootTableInventory);
                    OnSurvivorReturned?.Invoke();
                    Debug.Log("Survivor returned");
                }
            }
        }*/
    }

    public void Tick(float deltaTime)
    {
        elapsedTime += deltaTime;

        if (elapsedTime >= missionDuration)
        {
            ResolveMission();
        }            
    }
}
