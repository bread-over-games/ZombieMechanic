using UnityEngine;
using DG.Tweening;

public class SalvageTableAudio : MonoBehaviour
{
    [SerializeField] private Bench salvageTable;
    [SerializeField] private AudioSource loopAudio;
    [SerializeField] private AudioSource tickAudio;

    [SerializeField] private AudioClip salvageTickClip;
    [SerializeField] private AudioClip salvageFinishedClip;

    private void OnEnable()
    {
        SalvageTable.OnSalvageTick += SalvageTickAudio;
        SalvageTable.OnSalvageStart += AudioLoopStart;
        SalvageTable.OnSalvageStop += AudioLoopStop;
        SalvageTable.OnSalvageStop += SalvageFinishedAudio;
    }

    private void OnDisable()
    {
        SalvageTable.OnSalvageTick -= SalvageTickAudio;
        SalvageTable.OnSalvageStart -= AudioLoopStart;
        SalvageTable.OnSalvageStop -= AudioLoopStop;
        SalvageTable.OnSalvageStop -= SalvageFinishedAudio;
    }

    private void AudioLoopStart(Bench bench)
    {
        if (salvageTable != bench) return;
        loopAudio.Play();
    }

    private void AudioLoopStop(Bench bench)
    {
        if (salvageTable != bench) return;

        float audioLoopVolume = loopAudio.volume;
        loopAudio.DOFade(0f, 0.15f).OnComplete(() =>
        {
            loopAudio.Stop();
            loopAudio.volume = audioLoopVolume;
        });
    }

    private void SalvageTickAudio(Bench bench)
    {
        if (salvageTable != bench) return;
        tickAudio.clip = salvageTickClip;
        tickAudio.pitch = Random.Range(0.85f, 1.15f);
        tickAudio.Play();
    }

    private void SalvageFinishedAudio(Bench bench)
    {
        if (salvageTable != bench) return;
        tickAudio.clip = salvageFinishedClip;
        tickAudio.Play();
    }
}
