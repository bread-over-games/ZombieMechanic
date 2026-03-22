/// scriptable object for weapons

using UnityEngine;

[CreateAssetMenu(fileName = "NewWeapon", menuName = "Game/Weapon")]
public class WeaponData : ScriptableObject
{
    public Sprite weaponVisual;
    public GameObject weaponVisualPrefab;
    public string weaponName;
    public int minimalBaseDamage;
    public int maximalBaseDamage;
    public int maxDurability;
}
