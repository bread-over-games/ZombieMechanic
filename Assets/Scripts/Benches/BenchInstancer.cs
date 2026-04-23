using UnityEngine;

public class BenchInstancer : MonoBehaviour
{
    [SerializeField] private GameObject benchToSpawn;
    [SerializeField] private Transform spawnParent;
    [SerializeField] private Transform objectSpawnLocation; // some benches require secondary location for spawning objects

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
        
        GameObject spawnedBench = Instantiate(benchToSpawn, gameObject.transform.position, gameObject.transform.rotation, spawnParent);
        if (spawnedBench.TryGetComponent<Armory>(out Armory spawnedArmory))
        {
            spawnedArmory.SetSurvivorSpawnLocation(objectSpawnLocation);
        }
        
        Destroy(gameObject);
    }
}
