///
/// Visual representation of the weapon in the world (on benches, etc.)
///

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WeaponWorld : MonoBehaviour
{
    public static GameObject SpawnWeaponWorld(Vector3 position, Weapon weapon, Transform parent) // spawns weapon into world
    {        
        return Instantiate(weapon.GetObjectGameObject(), position, parent.rotation, parent);
    }
}
