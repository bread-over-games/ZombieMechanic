/// scriptable object for weapons

using UnityEngine;

[CreateAssetMenu(fileName = "NewWeapon", menuName = "Game/Weapon")]
public class WeaponData : ScriptableObject
{    
    public int baseDamage;
    public int maxDurability;
}
