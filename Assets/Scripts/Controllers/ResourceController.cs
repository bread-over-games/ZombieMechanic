using UnityEngine;

public class ResourceController : MonoBehaviour
{
    private int salvageAmount; // used for repairing
    private int upgradePartsAmount; // used for upgrading

    public static ResourceController Instance { get; private set; }
    
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public bool CanRepair()
    {
        if (salvageAmount > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void ChangeSalvageAmount(int amount)
    {
        salvageAmount += amount;

        if (salvageAmount <= 0)
        {
            salvageAmount = 0;
        }
    }
}
