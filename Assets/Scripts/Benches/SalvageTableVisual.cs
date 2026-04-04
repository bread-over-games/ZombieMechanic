using UnityEngine;

public class SalvageTableVisual : MonoBehaviour
{
    public ParticleSystem angleGrinderSparks;

    private void Awake()
    {
        StartAngleGrinderSparks();
    }

    private void OnEnable()
    {
        SalvageTable.OnSalvageStart += StartAngleGrinderSparks;
        SalvageTable.OnSalvageStop += StopAngleGrinderSparks;
    }

    private void OnDisable()
    {
        SalvageTable.OnSalvageStart -= StartAngleGrinderSparks;
        SalvageTable.OnSalvageStop -= StopAngleGrinderSparks;
    }

    private void StartAngleGrinderSparks()
    {
        angleGrinderSparks.Play();
    }

    private void StopAngleGrinderSparks()
    {
        angleGrinderSparks.Stop();
    }
}
