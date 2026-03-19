using UnityEngine;

public class AntibioticsWorld : MonoBehaviour
{
    public static GameObject SpawnAntibioticsWorld(Vector3 position, Antibiotics antibiotics, Transform parent) // spawns weapon into world
    {
        return Instantiate(antibiotics.GetObjectGameObject(), position, parent.rotation, parent);
    }
}
