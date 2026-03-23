using UnityEngine;

public static class MissionCalculator
{
    public static MissionEstimate EstimateMission(Weapon weaponToEquip, Backpack backpackToEquip, Armor armorToEquip)
    {
        int missionDuration = (int)CalculateMissionDuration(weaponToEquip, backpackToEquip, armorToEquip); 

        return new MissionEstimate
        {
            estimatedDuration = missionDuration,
            estimatedLootQualityMinimal = CalculateLootQualityMinimal(missionDuration),
            estimatedLootQualityMaximal = CalculateLootQualityMaximal(CalculateLootQualityMinimal(missionDuration)),
            estimatedZombiesKills = CalculateZombiesKills(missionDuration, weaponToEquip),
            estimatedGearWear = EstimateGearWear(missionDuration),
            estimatedLootAmount = EstimateLootAmount(backpackToEquip, weaponToEquip)
        };
    }

    public static MissionResult MissionResults(Weapon weaponToEquip, Backpack backpackToEquip, Armor armorToEquip)
    {
        int missionDuration = (int)CalculateMissionDuration(weaponToEquip, backpackToEquip, armorToEquip);

        return new MissionResult
        {
            duration = missionDuration,
            lootQualityMinimal = CalculateLootQualityMinimal(missionDuration),
            lootQualityMaximal = CalculateLootQualityMaximal(CalculateLootQualityMinimal(missionDuration)),
            zombiesKilled = CalculateZombiesKills(missionDuration, weaponToEquip),
            weaponWear = CalculateWeaponWear(missionDuration),
            armorWear = CalculateArmorWear(missionDuration),
            backpackWear = CalculateBackpackWear(missionDuration),
            lootAmount = CalculateLootAmount(backpackToEquip, weaponToEquip)
        };
    }

    private static int EstimateGearWear(int missionDuration)
    {
        float weaponWear = (missionDuration / 100f) * MissionController.Instance.loadoutWearWeaponWeight;
        float backpackWear = (missionDuration / 100f) * MissionController.Instance.loadoutWearBackpackWeight;
        float armorWear = (missionDuration / 100f) * MissionController.Instance.loadoutWearArmorWeight;

        int gearWear = (int)((weaponWear + backpackWear + armorWear)/3f);
        return gearWear;
    }

    private static int CalculateWeaponWear(int missionDuration)
    {
        int weaponWear = (int)((missionDuration / 100f) * MissionController.Instance.loadoutWearWeaponWeight);
        return weaponWear;
    }

    private static int CalculateArmorWear(int missionDuration)
    {
        int armorWear = (int)((missionDuration / 100f) * MissionController.Instance.loadoutWearArmorWeight);
        return armorWear;
    }

    private static int CalculateBackpackWear(int missionDuration)
    {
        int backpackWear = (int)((missionDuration / 100f) * MissionController.Instance.loadoutWearBackpackWeight);
        return backpackWear;
    }

    private static int CalculateLootAmount(Backpack backpackToEquip, Weapon weaponToEquip)
    {
        int lootAmount = 1;

        if (weaponToEquip == null)
        {
            lootAmount = Random.Range(0,2);
        }

        if (weaponToEquip != null && backpackToEquip == null)
        {
            lootAmount = Random.Range(1, 3);
        }

        if (weaponToEquip != null && backpackToEquip != null)        
        {
            lootAmount = backpackToEquip.backpackSize;
        }

        return lootAmount;            
    }

    private static int EstimateLootAmount(Backpack backpackToEquip, Weapon weaponToEquip)
    {
        int lootAmount = 1;

        if (weaponToEquip == null)
        {
            lootAmount = 0;
        }

        if (weaponToEquip != null && backpackToEquip == null)
        {
            lootAmount = 1;
        }

        if (weaponToEquip != null && backpackToEquip != null)
        {
            lootAmount = backpackToEquip.backpackSize;
        }

        return lootAmount;
    }

    private static float CalculateLootQualityMinimal(int missionDuration)
    {
        float lootQuality;

        lootQuality = (missionDuration / MissionController.Instance.missionMaximumLength) * 100;
        if (lootQuality < MissionController.Instance.minimalLootQuality)
        {
            lootQuality = MissionController.Instance.minimalLootQuality;
        }

        return lootQuality;
    }

    private static float CalculateLootQualityMaximal(float minimalQuality)
    {
        float maximalLootQuality = minimalQuality * 2.5f;

        if (maximalLootQuality > 100)
        {
            maximalLootQuality = 100;
        }        

        return maximalLootQuality;
    }

    private static int CalculateZombiesKills(int missionDuration, Weapon equippedWeapon)
    {
        float zombieKillsDivisor = 35f; // Divisor - the lower the value the higher the kill count
        float weaponKills;
        int weaponDamage;
        if (equippedWeapon == null)
        {
            weaponDamage = 1;
            zombieKillsDivisor = 100f;
        } else
        {
            weaponDamage = equippedWeapon.baseDamage;
        }

        weaponKills = missionDuration * (weaponDamage / 50f);
        int zombiesKills = (int)(missionDuration * missionDuration / zombieKillsDivisor); // basic formula, will be improved with weapon damage
        zombiesKills += (int)weaponKills;

        if (zombiesKills < 1)
        {
            zombiesKills = 1;
        }

        return zombiesKills;
    }

    private static float CalculateMissionDuration(Weapon weaponToEquip, Backpack backpackToEquip, Armor armorToEquip)
    {
        float weaponTimeValue;
        float armorTimeValue;
        float backpackTimeValue;
        

        if (weaponToEquip == null)
        {
            weaponTimeValue = 2f;
        }
        else
        {
            weaponTimeValue = ((float)weaponToEquip.currentDurability / 100) * MissionController.Instance.missionLengthWeaponWeight;
        }

        if (armorToEquip == null)
        {
            armorTimeValue = 2f;
        }
        else
        {
            armorTimeValue = ((float)armorToEquip.currentDurability / 100) * MissionController.Instance.missionLengthArmorWeight;
        }

        if (backpackToEquip == null)
        {
            backpackTimeValue = 2f;
        }
        else
        {
            backpackTimeValue = ((float)backpackToEquip.currentDurability / 100) * MissionController.Instance.missionLengthBackpackWeight;
        }

        float missionDuration = (weaponTimeValue + armorTimeValue + backpackTimeValue) * MissionController.Instance.missionLengthMultiplier;

        if (missionDuration > MissionController.Instance.missionMaximumLength)
        {
            missionDuration = MissionController.Instance.missionMaximumLength;
        }

        if (weaponToEquip == null)
        {
            if (missionDuration > 20)
            {
                missionDuration = 20;
            }
        }
        
        return missionDuration;
    }

}
