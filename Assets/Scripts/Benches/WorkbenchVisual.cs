using UnityEngine;
using DG.Tweening;

public class WorkbenchVisual : MonoBehaviour
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
        Workbench.OnRepairStart += StartWeldSparks;
        Workbench.OnRepairStop += StopWeldSparks;
    }

    private void OnDisable()
    {
        Workbench.OnRepairStart -= StartWeldSparks;
        Workbench.OnRepairStop -= StopWeldSparks;
    }

    private void StartWeldSparks()
    {
        angleGrinderSparks.Play();
        weldLight.enabled = true;
        FlickerNext();
    }

    private void StopWeldSparks()
    {
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
