using UnityEngine;

public class ArmorAssets : MonoBehaviour
{
    public static ArmorAssets Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    public ArmorData balisticVestSO;
    public ArmorData riotGearSO;
    public ArmorData leatherJacketSO;
}
