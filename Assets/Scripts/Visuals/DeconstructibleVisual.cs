using DG.Tweening;
using UnityEngine;

public class DeconstructibleVisual : MonoBehaviour
{
    public ParticleSystem angleGrinderSparks;

    [Header("Light intensity Range")]
    public float minIntensity = 0.5f;
    public float maxIntensity = 3f;

    [Header("Light speed")]
    public float minInterval = 0.03f;
    public float maxInterval = 0.12f;

    public Light grinderLight;

    private void OnEnable()
    {
        ObjectDeconstruction.OnDeconstructionStart += StartVisual;
        ObjectDeconstruction.OnDeconstructionStop += StopVisual;
    }

    private void OnDisable()
    {
        ObjectDeconstruction.OnDeconstructionStart -= StartVisual;
        ObjectDeconstruction.OnDeconstructionStop -= StopVisual;
    }

    private void StartVisual(GameObject obj)
    {
        if (obj != gameObject) return;
        angleGrinderSparks.Play();
        grinderLight.enabled = true;
        FlickerNext();
    }

    private void StopVisual(GameObject obj)
    {
        if (obj != gameObject) return;
        angleGrinderSparks.Stop();
        grinderLight.enabled = false;
        StopFlicker();
    }

    private void FlickerNext()
    {
        float targetIntensity = Random.Range(minIntensity, maxIntensity);
        float duration = Random.Range(minInterval, maxInterval);

        grinderLight.DOIntensity(targetIntensity, duration).SetEase(Ease.InOutSine).OnComplete(FlickerNext);
    }

    private void StopFlicker()
    {
        grinderLight.DOKill();
    }

    void OnDestroy()
    {
        grinderLight.DOKill();
    }
}
