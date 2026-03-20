using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
[System.Serializable]
public class Mission
{
    public Survivor survivor; // current survivor on the mission    
    public bool isComplete = false;
    [SerializeReference] public Weapon equippedWeapon;
    [SerializeReference] public Armor equippedArmor;
    [SerializeReference] public Backpack equippedBackpack;
    [SerializeReference] public Inventory missionInventory;

    private float missionDuration;
    private float elapsedTime;
    private float lootQualityMultiplier; // based on run length and affects the quality of equipment the survivors bring back, in percents
    private int zombiesKilled;

    public Mission(/*Survivor missionSurvivor, */Weapon weaponToEquip, Backpack backpackToEquip, Armor armorToEquip, Inventory inventoryOfMission)
    {
        // survivor = missionSurvivor;
        equippedWeapon = weaponToEquip;
        equippedBackpack = backpackToEquip;
        equippedArmor = armorToEquip;   
        missionInventory = inventoryOfMission;

        elapsedTime = 0f;

        CalculateMissionDuration();
    }

    private void CalculateZombiesKilled()
    {
        zombiesKilled = (int)(missionDuration * missionDuration / 35f); // basic formula, will be improved with weapon damage. Divisor - the lower the value the higher the kill count
        ZombiesController.Instance.AddKilledZombies(zombiesKilled);
    }

    private void CalculateMissionDuration()
    {
        float weaponTimeValue;
        float armorTimeValue;
        float backpackTimeValue;

        if (equippedWeapon == null)
        {
            weaponTimeValue = 2f;
        } else
        {
            weaponTimeValue = ((float)equippedWeapon.currentDurability / 100) * MissionController.Instance.missionLengthWeaponWeight;
        }

        if (equippedArmor == null)
        {
            armorTimeValue = 2f;
        } else
        {
            armorTimeValue = ((float)equippedArmor.currentDurability / 100) * MissionController.Instance.missionLengthArmorWeight;
        }

        if (equippedBackpack == null)
        {
            backpackTimeValue = 2f;
        } else
        {
            backpackTimeValue = ((float)equippedBackpack.currentDurability / 100) * MissionController.Instance.missionLengthBackpackWeight;
        }

        missionDuration = (weaponTimeValue + armorTimeValue + backpackTimeValue) * MissionController.Instance.missionLengthMultiplier;
    }

    public void ResolveMission()
    {
        CalculateZombiesKilled();
        ApplyWearToLoadout();
        ReturnSurvivorLoadout();
        GenerateLootQuality();
        GenerateLoot();
        isComplete = true;
        //Debug.Log(survivor.survivorName + " has returned from the mission!");
    }

    private void GenerateLootQuality()
    {
        lootQualityMultiplier = (missionDuration / MissionController.Instance.missionMaximumLength) * 100;
        if (lootQualityMultiplier < MissionController.Instance.minimalLootQuality)
        {
            lootQualityMultiplier = MissionController.Instance.minimalLootQuality;
        }
    }

    private void GenerateLoot()
    {
        if (equippedBackpack == null) 
        {
            ObjectGenerator.Instance.GenerateLoot(this, lootQualityMultiplier); // if player has no backpack generate two items
            ObjectGenerator.Instance.GenerateLoot(this, lootQualityMultiplier);
        } 
        else
        {
            for (int i = 0; i < equippedBackpack.backpackSize; i++)
            {
                ObjectGenerator.Instance.GenerateLoot(this, lootQualityMultiplier);
            }
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
            float weaponWear = (missionDuration / 100) * MissionController.Instance.loadoutWearWeaponWeight;

            if (weapon.DamageObject((int)weaponWear))
            {
                Debug.Log(weapon.objectName + " destroyed on a mission");
            }            
        }

        if (equippedBackpack is Backpack backpack)
        {
            float backpackWear = (missionDuration / 100) * MissionController.Instance.loadoutWearBackpackWeight;            

            if (backpack.DamageObject((int)backpackWear))
            {
                Debug.Log(backpack.objectName + " destroyed on a mission");
            }
        }

        if (equippedArmor is Armor armor)
        {
            float armorWear = (missionDuration / 100) * MissionController.Instance.loadoutWearArmorWeight;

            if (armor.DamageObject((int)armorWear))
            {
                Debug.Log(armor.objectName + " destroyed on a mission");
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
