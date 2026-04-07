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
        BenchConstruction.OnConstructionFinished -= SpawnBench;
    }

    private void SpawnBench(GameObject finishedBench)
    {
        if (this == null) return;
        if (finishedBench == null) return;
        if (finishedBench != gameObject) return;
        Instantiate(benchToSpawn, gameObject.transform.position, gameObject.transform.rotation, spawnParent);
        Destroy(gameObject);
    }
}
