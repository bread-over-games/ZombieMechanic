using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System;

[System.Serializable]
public class Mission
{
    public enum MissionType
    {
        Scavenge,
        Extermination,
        Antibiotics
    }

    public Survivor survivor; // current survivor on the mission    
    public bool isComplete = false;
    [SerializeReference] public Weapon equippedWeapon;
    [SerializeReference] public Armor equippedArmor;
    [SerializeReference] public Backpack equippedBackpack;
    [SerializeReference] public Inventory missionInventory;
    [SerializeReference] public MissionType missionType;

    public Armory armoryOwner;

    [HideInInspector] public float elapsedTime;

    public float missionDuration;
    private float lootQualityMinimal;
    private float lootQualityMaximal;
    private int zombiesKilled;
    public int weaponWear;
    public int armorWear;
    public int backpackWear;
    public int lootAmount;

    public Mission(/*Survivor missionSurvivor, */Weapon weaponToEquip, Backpack backpackToEquip, Armor armorToEquip, Inventory inventoryOfMission, Armory missionOwner, MissionType currentMissionType)
    {
        // survivor = missionSurvivor;
        equippedWeapon = weaponToEquip;
        equippedBackpack = backpackToEquip;
        equippedArmor = armorToEquip;   
        missionInventory = inventoryOfMission;
        armoryOwner = missionOwner;
        missionType = currentMissionType;

        elapsedTime = 0f;

        MissionResult missionResult = MissionCalculator.MissionResults(equippedWeapon, equippedBackpack, equippedArmor, missionType);
        missionDuration = missionResult.duration;
        lootQualityMinimal = missionResult.lootQualityMinimal;
        lootQualityMaximal = missionResult.lootQualityMaximal;
        zombiesKilled = missionResult.zombiesKilled;
        lootAmount = missionResult.lootAmount;
        weaponWear = missionResult.weaponWear;
        armorWear = missionResult.armorWear;
        backpackWear = missionResult.backpackWear;
    }

    public void ResolveMission()
    {
        ZombiesController.Instance.AddKilledZombies(zombiesKilled);
        XPCounter.Instance.AddZombieKillXP(zombiesKilled);
        ApplyWearToLoadout();
        ReturnSurvivorLoadout();

        GenerateLoot();
        isComplete = true;
        armoryOwner.MakeArmoryAvailableForMission();
        //Debug.Log(survivor.survivorName + " has returned from the mission!");
    }


    private void GenerateLoot()
    {
        for (int i = 0; i < lootAmount; i++)
        {
            ObjectGenerator.Instance.GenerateLoot(this, lootQualityMinimal, lootQualityMaximal, missionType);
        }
    }

    private void ReturnSurvivorLoadout()
    {
        if (equippedWeapon is Weapon weapon)
        {
            if (weapon.currentDurability <= 0)
            {
                equippedWeapon = null;
            }
            else
            {
                missionInventory.ReceiveObject(equippedWeapon);
            }
        }

            if (equippedArmor is Armor armor)
        {
            if (equippedArmor.currentDurability <= 0)
            {
                equippedArmor = null;
            }
            else
            {
                missionInventory.ReceiveObject(equippedArmor);
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
                missionInventory.ReceiveObject(equippedBackpack);
            }
        }        
    }

    private void ApplyWearToLoadout()
    {
        if (equippedWeapon is Weapon weapon)
        {
            if (!weapon.canBeDestroyed)
            {                
                int damageToApply = Mathf.Min(weaponWear, weapon.currentDurability - 1); // Weapon is safe — clamp damage so it survives with at least 1 durability
                weapon.DamageObject(damageToApply);
            } else
            {
                if (weapon.DamageObject(weaponWear))
                {
                    Debug.Log(weapon.objectName + " destroyed on a mission");
                }
            }                     
        }

        if (equippedBackpack is Backpack backpack)
        {
            if (!backpack.canBeDestroyed)
            {
                int damageToApply = Mathf.Min(backpackWear, backpack.currentDurability - 1);
                backpack.DamageObject(damageToApply);
            }
            else
            {
                if (backpack.DamageObject(backpackWear))
                {
                    Debug.Log(backpack.objectName + " destroyed on a mission");
                }
            }
        }

        if (equippedArmor is Armor armor)
        {
            if (!armor.canBeDestroyed)
            {
                int damageToApply = Mathf.Min(armorWear, armor.currentDurability - 1);
                armor.DamageObject(damageToApply);
            }
            else
            {
                if (armor.DamageObject(armorWear))
                {
                    Debug.Log(armor.objectName + " destroyed on a mission");
                }
            }
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
