using UnityEngine;

public class BenchInstancer : MonoBehaviour
{
    [SerializeField] private GameObject benchToSpawn;
    [SerializeField] private Transform spawnParent;

    private void OnEnable()
    {
        BenchConstruction.OnConstructionFinished += SpawnBench;
    }

    private void OnDisable()
    {
        BenchConstruction.OnConstructionFinished += SpawnBench;
    }

    private void SpawnBench()
    {
        Instantiate(benchToSpawn, gameObject.transform.position, gameObject.transform.rotation, spawnParent);
        DestroyObject();
    }

    private void DestroyObject()
    {
        Destroy(gameObject);
    }
}
