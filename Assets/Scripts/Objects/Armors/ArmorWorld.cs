using UnityEngine;

public class ArmorWorld : MonoBehaviour
{
    public static GameObject SpawnArmorWorld(Vector3 position, Armor armor, Transform parent) // spawns weapon into world
    {
        return Instantiate(armor.GetObjectGameObject(), position, parent.rotation, parent);
    }
}
