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
            estimatedZombiesKills = CalculateZombiesKills(missionDuration),
            estimatedGearWear = EstimateGearWear(missionDuration),
            estimatedLootAmount = CalculateLootAmount(backpackToEquip)
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
            zombiesKilled = CalculateZombiesKills(missionDuration),
            weaponWear = CalculateWeaponWear(missionDuration),
            armorWear = CalculateArmorWear(missionDuration),
            backpackWear = CalculateBackpackWear(missionDuration),
            lootAmount = CalculateLootAmount(backpackToEquip)
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

    private static int CalculateLootAmount(Backpack backpackToEquip)
    {
        if (backpackToEquip == null)
        {
            return 2;
        } else
        {
            return backpackToEquip.backpackSize;
        }
            
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
        float maximalLootQuality = minimalQuality * 3f;

        if (maximalLootQuality > 100)
        {
            maximalLootQuality = 100;
        }        

        return maximalLootQuality;
    }

    private static int CalculateZombiesKills(int missionDuration)
    {
        int zombiesKills = (int)(missionDuration * missionDuration / 35f); // basic formula, will be improved with weapon damage. Divisor - the lower the value the higher the kill count
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

        
        return missionDuration;
    }

}
