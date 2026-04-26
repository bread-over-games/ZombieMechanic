using DG.Tweening;
using UnityEngine;

public class SalvageTableVisual : MonoBehaviour
{
    public ObjectDisplay salvageTableObjDisplay;
    public ParticleSystem angleGrinderSparks;

    [Header("Light intensity Range")]
    public float minIntensity = 0.5f;
    public float maxIntensity = 3f;

    [Header("Light speed")]
    public float minInterval = 0.03f;
    public float maxInterval = 0.12f;

    public Light grindLight;

    private void OnEnable()
    {
        SalvageTable.OnSalvageStart += StartAngleGrinderSparks;
        SalvageTable.OnSalvageStop += StopAngleGrinderSparks;
        SalvageTable.OnSalvageTick += SalvageTickEffect;
    }

    private void OnDisable()
    {
        SalvageTable.OnSalvageStart -= StartAngleGrinderSparks;
        SalvageTable.OnSalvageStop -= StopAngleGrinderSparks;
        SalvageTable.OnSalvageTick -= SalvageTickEffect;
    }

    private void SalvageTickEffect()
    {
        if (salvageTableObjDisplay.currentObjects[0] == null) return;
        salvageTableObjDisplay.currentObjects[0].GetComponent<ObjectEffects>().Shake();
    }

    private void StartAngleGrinderSparks()
    {
        angleGrinderSparks.Play();
        grindLight.enabled = true;
        FlickerNext();
    }

    private void StopAngleGrinderSparks()
    {
        angleGrinderSparks.Stop();
        grindLight.enabled = false;
        StopFlicker();
    }

    private void FlickerNext()
    {
        float targetIntensity = Random.Range(minIntensity, maxIntensity);
        float duration = Random.Range(minInterval, maxInterval);

        grindLight.DOIntensity(targetIntensity, duration).SetEase(Ease.InOutSine).OnComplete(FlickerNext);
    }

    private void StopFlicker()
    {
        grindLight.DOKill();
    }

    void OnDestroy()
    {
        grindLight.DOKill();
    }
}
