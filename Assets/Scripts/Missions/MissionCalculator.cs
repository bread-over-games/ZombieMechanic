using UnityEngine;
using static Mission;

public static class MissionCalculator
{
    private static bool negateWeaponWear = false;
    private static bool negateArmorWear = false;
    private static bool negateBackpackWear = false;

    public static void Initialize()
    {
        LuckyBastardHandler.OnNoWeaponWear += SetNegateWeaponWear;
        LuckyBastardHandler.OnNoArmorWear += SetNegateArmorWear;
        LuckyBastardHandler.OnNoBackpackWear += SetNegateBackpackWear;
    }

    public static void Cleanup()
    {
        LuckyBastardHandler.OnNoWeaponWear -= SetNegateWeaponWear;
        LuckyBastardHandler.OnNoArmorWear -= SetNegateArmorWear;
        LuckyBastardHandler.OnNoBackpackWear -= SetNegateBackpackWear;
    }

    private static void SetNegateWeaponWear()
    {
        negateWeaponWear = true; 
    }

    private static void SetNegateArmorWear()
    {
        negateArmorWear = true;
    }

    private static void SetNegateBackpackWear()
    {
        negateBackpackWear = true;
    }

    public static MissionEstimate EstimateMission(Weapon weaponToEquip, Backpack backpackToEquip, Armor armorToEquip, Mission.MissionType missionType)
    {
        int missionDuration = (int)CalculateMissionDuration(weaponToEquip, backpackToEquip, armorToEquip); 

        return new MissionEstimate
        {
            estimatedDuration = missionDuration,
            estimatedLootQualityMinimal = CalculateLootQualityMinimal(missionDuration, armorToEquip),
            estimatedLootQualityMaximal = CalculateLootQualityMaximal(CalculateLootQualityMinimal(missionDuration, armorToEquip)),
            estimatedZombiesKills = CalculateZombiesKills(missionDuration, weaponToEquip, missionType),
            estimatedGearWear = (CalculateWeaponWear(missionDuration, missionType) + CalculateArmorWear(missionDuration, missionType) + CalculateBackpackWear(missionDuration, missionType)) / 3,
            estimatedLootAmount = EstimateLootAmount(backpackToEquip, weaponToEquip, missionType)
        };
    }

    public static MissionResult MissionResults(Weapon weaponToEquip, Backpack backpackToEquip, Armor armorToEquip, Mission.MissionType missionType)
    {
        int missionDuration = (int)CalculateMissionDuration(weaponToEquip, backpackToEquip, armorToEquip);

        return new MissionResult
        {
            duration = missionDuration,
            lootQualityMinimal = CalculateLootQualityMinimal(missionDuration, armorToEquip),
            lootQualityMaximal = CalculateLootQualityMaximal(CalculateLootQualityMinimal(missionDuration, armorToEquip)),
            zombiesKilled = CalculateZombiesKills(missionDuration, weaponToEquip, missionType),
            weaponWear = CalculateWeaponWear(missionDuration, missionType),
            armorWear = CalculateArmorWear(missionDuration, missionType),
            backpackWear = CalculateBackpackWear(missionDuration, missionType),
            lootAmount = CalculateLootAmount(backpackToEquip, weaponToEquip, missionType)
        };
    }

    private static int EstimateGearWear(int missionDuration)
    {
        float weaponWear = missionDuration * 0.25f * ValueModifiers.Instance.gearWearModifier;
        float backpackWear = missionDuration * 0.25f * ValueModifiers.Instance.gearWearModifier;
        float armorWear = missionDuration * 0.25f * ValueModifiers.Instance.gearWearModifier;

        int gearWear = (int)((weaponWear + backpackWear + armorWear)/3f);
        return gearWear;
    }

    private static int CalculateWeaponWear(int missionDuration, MissionType missionType)
    {
        if (negateWeaponWear)
        {
            negateWeaponWear = false;
            return 0;
        }

        float missionTypeMultiplier = missionType switch
        {
            Mission.MissionType.Extermination => 0.65f,
            _ => 0.05f,
        };

        float raw = missionDuration * missionTypeMultiplier * ValueModifiers.Instance.gearWearModifier;

        return Mathf.RoundToInt(raw);
    }

    private static int CalculateArmorWear(int missionDuration, MissionType missionType)
    {
        if (negateArmorWear)
        {
            negateArmorWear = false;
            return 0;
        }

        float missionTypeMultiplier = missionType switch
        {
            Mission.MissionType.Extermination => 0.5f,
            _ => 0.05f,
        };

        float raw = missionDuration * missionTypeMultiplier * ValueModifiers.Instance.gearWearModifier;

        return Mathf.RoundToInt(raw);
    }

    private static int CalculateBackpackWear(int missionDuration, MissionType missionType)
    {
        if (negateBackpackWear)
        {
            negateBackpackWear = false;
            return 0;
        }

        float missionTypeMultiplier = missionType switch
        {
            Mission.MissionType.Extermination => 0.3f,
            _ => 0.05f,
        };

        float raw = missionDuration * missionTypeMultiplier * ValueModifiers.Instance.gearWearModifier;

        return Mathf.RoundToInt(raw);
    }

    private static int CalculateLootAmount(Backpack backpackToEquip, Weapon weaponToEquip, MissionType missionType)
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

        if (missionType == MissionType.Extermination)
        {
            lootAmount = Random.Range(0, 2); 
        }

        return lootAmount;            
    }

    private static int EstimateLootAmount(Backpack backpackToEquip, Weapon weaponToEquip, MissionType missionType)
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

        if (missionType == MissionType.Extermination)
        {
            lootAmount = Random.Range(0, 2); 
        }

        return lootAmount;
    }

    private static float CalculateLootQualityMinimal(int missionDuration, Armor eqippedArmor)
    {
        float lootQuality;

        lootQuality = ((missionDuration / MissionController.Instance.missionMaximumLength) * 100);

        if (eqippedArmor != null)
        {
            lootQuality += eqippedArmor.lootQualityBonus;
        }

        if (lootQuality < MissionController.Instance.minimalLootQuality)
        {
            lootQuality = MissionController.Instance.minimalLootQuality;
        }

        return lootQuality;
    }

    private static float CalculateLootQualityMaximal(float minimalQuality)
    {
        float maximalLootQuality = minimalQuality * 2f;

        if (maximalLootQuality > 100)
        {
            maximalLootQuality = 100;
        }        

        return maximalLootQuality;
    }

    private static int CalculateZombiesKills(int missionDuration, Weapon equippedWeapon, Mission.MissionType selectedMissionType)
    {
        {
            float missionTypeDamageMultiplier = selectedMissionType switch
            {
                Mission.MissionType.Extermination => 0.95f,
                Mission.MissionType.Scavenge => 0.15f,
                Mission.MissionType.Antibiotics => 0.15f,
                _ => 1f
            };

            int weaponDamage;
            if (equippedWeapon == null)
            {
                weaponDamage = 1;
            }
            else
            {
                weaponDamage = equippedWeapon.baseDamage;
            }

            float zombiesKills = weaponDamage * Mathf.Sqrt(missionDuration) * missionTypeDamageMultiplier * ValueModifiers.Instance.zombieKillsModifier;
            return Mathf.RoundToInt(zombiesKills);
        }
    }

    private static float CalculateMissionDuration(Weapon weaponToEquip, Backpack backpackToEquip, Armor armorToEquip)
    {
        float weaponTimeValue;
        float armorTimeValue;
        float backpackTimeValue;
        

        if (weaponToEquip == null)
        {
            weaponTimeValue = 1f;
        }
        else
        {
            weaponTimeValue = ((float)weaponToEquip.currentDurability / 100) * MissionController.Instance.missionLengthWeaponWeight;
        }

        if (armorToEquip == null)
        {
            armorTimeValue = 1f;
        }
        else
        {
            armorTimeValue = ((float)armorToEquip.currentDurability / 100) * MissionController.Instance.missionLengthArmorWeight;
        }

        if (backpackToEquip == null)
        {
            backpackTimeValue = 1f;
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
