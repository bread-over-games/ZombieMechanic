using UnityEngine;
using System;

public class ResourceController : MonoBehaviour
{
    private int sparePartsAmount; // used for repairing
    private int sparePartsLimit;
    private int antibioticsAmount;    

    public static Action OnSparePartsAmountChange;
    public static Action OnSparePartsLimitReached;
    public static Action OnNoSpareParts;

    public static Action OnAntibioticsAmountChange;
    public static Action OnNoAntibiotics;
    public static ResourceController Instance { get; private set; }

    private void OnEnable()
    {
        StorageRack.OnStorageRackBuilt += IncreaseSparePartsLimit;
    }

    private void OnDisable()
    {
        StorageRack.OnStorageRackBuilt -= IncreaseSparePartsLimit;
    }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        GiveStarterAntibiotics();
    }

    private void IncreaseSparePartsLimit(int amount)
    {
        sparePartsLimit += amount;
        OnSparePartsAmountChange?.Invoke();
    }

    private void GiveStarterAntibiotics()
    {
        antibioticsAmount = 2;
        OnAntibioticsAmountChange?.Invoke();
    }

    public bool CanRepair(int requiredSpareParts)
    {
        if (sparePartsAmount >= requiredSpareParts)
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

    public int GetSparePartsLimit()
    {
        return sparePartsLimit;
    }

    public int GetAntibioticsAmount()
    {
        return antibioticsAmount; 
    }

    public bool CheckSparePartsLimit(int amount)
    {
        int sparePartsHolder = sparePartsAmount + amount;

        if (sparePartsHolder >= sparePartsLimit)
        {
            OnSparePartsLimitReached?.Invoke();
            return false; // no space left in storage racks
        } else
        {
            return true; // there is a space in storage racks
        }
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
            OnNoAntibiotics?.Invoke();
        }

        OnAntibioticsAmountChange?.Invoke();
    }
}
