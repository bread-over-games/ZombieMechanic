/// this holds references to scriptable objects for weapons

using UnityEngine;

public class WeaponAssets : MonoBehaviour
{
    public static WeaponAssets Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    public WeaponData baseballBatSO;
    public WeaponData crowbarSO;
}
