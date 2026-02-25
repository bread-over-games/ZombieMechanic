///
/// Visual representation of the weapon in the world (on benches, etc.)
///

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WeaponWorld : MonoBehaviour
{
    public static WeaponWorld SpawnWeaponWorld(Vector3 position, Weapon weapon, Transform parent) // spawns weapon into world
    {
        Transform spawnedWeapon = Instantiate(WeaponAssets.Instance.weaponWorldPrefab, position, Quaternion.identity, parent);

        WeaponWorld weaponWorld = spawnedWeapon.GetComponent<WeaponWorld>();
        weaponWorld.SetWeapon(weapon);

        return weaponWorld;
    }

    public Weapon weapon;
    public void SetWeapon (Weapon weapon)
    {
        this.weapon = weapon;
        weapon.SetValues();
        Instantiate(weapon.GetWeaponGameObject(), transform);
    }
}
