using DG.Tweening;
using UnityEngine;

public class BenchConstruictibleVisual : MonoBehaviour
{
    public ParticleSystem angleGrinderSparks;

    [Header("Light intensity Range")]
    public float minIntensity = 0.5f;
    public float maxIntensity = 3f;

    [Header("Light speed")]
    public float minInterval = 0.03f;
    public float maxInterval = 0.12f;

    public Light weldLight;

    private void OnEnable()
    {
        BenchConstruction.OnConstructionStart += StartWeldSparks;
        BenchConstruction.OnConstructionStop += StopWeldSparks;
    }

    private void OnDisable()
    {
        BenchConstruction.OnConstructionStart -= StartWeldSparks;
        BenchConstruction.OnConstructionStop -= StopWeldSparks;
    }

    private void StartWeldSparks(GameObject obj)
    {
        if (obj != gameObject) return;
        angleGrinderSparks.Play();
        weldLight.enabled = true;
        FlickerNext();
    }

    private void StopWeldSparks(GameObject obj)
    {
        if (obj != gameObject) return;
        angleGrinderSparks.Stop();
        weldLight.enabled = false;
        StopFlicker();
    }

    private void FlickerNext()
    {
        float targetIntensity = Random.Range(minIntensity, maxIntensity);
        float duration = Random.Range(minInterval, maxInterval);

        weldLight.DOIntensity(targetIntensity, duration).SetEase(Ease.InOutSine).OnComplete(FlickerNext);
    }

    private void StopFlicker()
    {
        weldLight.DOKill();
    }

    void OnDestroy()
    {
        weldLight.DOKill();
    }
}
