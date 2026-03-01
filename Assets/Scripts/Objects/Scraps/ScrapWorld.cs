using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ScrapWorld : MonoBehaviour
{
    public static GameObject SpawnScrapWorld(Vector3 position, Scrap scrap, Transform parent) // spawns weapon into world
    {
        return Instantiate(scrap.GetObjectGameObject(), position, parent.rotation, parent);
    }
}
