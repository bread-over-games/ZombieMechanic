///
/// Visual representation of the weapon in the world (on benches, etc.)
///

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BackpackWorld : MonoBehaviour
{
    public static GameObject SpawnBackpackWorld(Vector3 position, Backpack backpack, Transform parent) // spawns backpack into world
    {
        Debug.Log("backpack instantiated");
        return Instantiate(backpack.GetObjectGameObject(), position, parent.rotation, parent);

    }
}
