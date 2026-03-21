using UnityEngine;
using System;

public class ResourceController : MonoBehaviour
{
    private int sparePartsAmount; // used for repairing
    private int antibioticsAmount;

    public static Action OnSparePartsAmountChange;
    public static Action OnNoSpareParts;

    public static Action OnAntibioticsAmountChange;
    public static Action OnNoAntibiotics;

    public static ResourceController Instance { get; private set; }
    
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        GiveStarterAntibiotics();
    }

    private void GiveStarterAntibiotics()
    {
        antibioticsAmount = 2;
        OnAntibioticsAmountChange?.Invoke();
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

    public int GetAntibioticsAmount()
    {
        return antibioticsAmount; 
    }

    public void ChangeSparePartsAmount(int amount)
    {
        sparePartsAmount += amount;

        if (sparePartsAmount <= 0)
        {
            sparePartsAmount = 0;
            OnNoSpareParts?.Invoke();
        }

        OnSparePartsAmountChange?.Invoke();
    }

    public void ChangeAntibioticsAmount(int amount)
    {
        antibioticsAmount += amount;

        if (antibioticsAmount <= 0)
        {
            antibioticsAmount = 0;
            Debug.Log("Not enough antibiotics!");
            OnNoAntibiotics?.Invoke();
        }

        OnAntibioticsAmountChange?.Invoke();
    }
}
