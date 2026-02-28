/// this holds references to all assets neccessary to construct a weapon in UI or in world or in inventory

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

    public Transform weaponWorldPrefab;

    public Sprite baseballBatVisual;
    public Sprite crowbarVisual;

    public GameObject baseballBatVisualPrefab;
    public GameObject crowbarVisualPrefab;
}
