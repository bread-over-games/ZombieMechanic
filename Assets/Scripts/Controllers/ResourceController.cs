using UnityEngine;
using System;

public class ResourceController : MonoBehaviour
{
    private int sparePartsAmount; // used for repairing
    private int upgradePartsAmount; // used for upgrading

    public static Action OnSparePartsAmountChange;

    public static ResourceController Instance { get; private set; }
    
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public bool CanRepair(int requiredSalvage)
    {
        if (sparePartsAmount >= requiredSalvage)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public int GetSparePartsAmount()
    {
        return sparePartsAmount;
    }

    public void ChangeSparePartsAmount(int amount)
    {
        sparePartsAmount += amount;

        if (sparePartsAmount <= 0)
        {
            sparePartsAmount = 0;
        }

        OnSparePartsAmountChange?.Invoke();
    }
}
