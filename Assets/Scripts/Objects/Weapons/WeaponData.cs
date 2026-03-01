/// scriptable object for weapons

using UnityEngine;

[CreateAssetMenu(fileName = "NewWeapon", menuName = "Game/Weapon")]
public class WeaponData : ScriptableObject
{
    public Sprite weaponVisual;
    public GameObject weaponVisualPrefab;
    public int baseDamage;
    public int maxDurability;
}
