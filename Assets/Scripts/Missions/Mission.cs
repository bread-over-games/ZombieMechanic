using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Mission
{
    public Survivor survivor; // current survivor on the mission    
    public bool isComplete = false;
    public Weapon equippedWeapon;
    public Armor equippedArmor;
    public Backpack equippedBackpack;

    public float missionDuration;
    public float elapsedTime;

    public Mission(/*Survivor missionSurvivor, */Weapon weaponToEquip, Backpack backpackToEquip, Armor armorToEquip)
    {
        // survivor = missionSurvivor;
        equippedWeapon = weaponToEquip;
        equippedBackpack = backpackToEquip;
        equippedArmor = armorToEquip;   

        elapsedTime = 0f;

        missionDuration = 10f; // for now, no calculation
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
        if (equippedWeapon is Weapon weapon && equippedWeapon.currentDurability <= 0)
        {
            equippedWeapon = null;
        }
        else
        {
            InventoriesController.Instance.armoryInventory.ReceiveObject(equippedWeapon);

        }

        if (equippedArmor is Armor armor)
        {
            if (equippedArmor.currentDurability <= 0)
            {
                equippedArmor = null;
            }
            else
            {
                InventoriesController.Instance.armoryInventory.ReceiveObject(equippedArmor);
            }
        }

        if (equippedBackpack is Backpack backpack)
        {
            if (equippedBackpack.currentDurability <= 0)
            {
                equippedBackpack = null;
            }
            else
            {
                InventoriesController.Instance.armoryInventory.ReceiveObject(equippedBackpack);
            }
        }
    }


    private void ApplyWearToLoadout()
    {
        if (equippedWeapon is Weapon weapon && equippedWeapon.DamageObject(5))
        {
            Debug.Log(equippedWeapon.weaponName + " destroyed on a mission");            
        }

        if (equippedBackpack is Backpack backpack && equippedBackpack.DamageObject(5))
        {
            Debug.Log(equippedBackpack.backpackName + " destroyed on a mission");
        }

        if (equippedArmor is Armor armor && equippedArmor.DamageObject(5))
        {
            Debug.Log(equippedArmor.armorName + " destroyed on a mission");
        }
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
