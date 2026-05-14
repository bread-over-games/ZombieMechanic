using DG.Tweening;
using UnityEngine;

public class WorkbenchAudio : MonoBehaviour
{
    [SerializeField] private Bench workbench;
    [SerializeField] private AudioSource loopAudio;
    [SerializeField] private AudioSource tickAudio;

    [SerializeField] private AudioClip repairTickClip;
    [SerializeField] private AudioClip repairFinishedClip;

    private void OnEnable()
    {
        Workbench.OnRepairTick += RepairTickAudio;
        Workbench.OnRepairStart += AudioLoopStart;
        Workbench.OnRepairStop += AudioLoopStop;
        Workbench.OnRepairStop += RepairFinishedAudio;
    }

    private void OnDisable()
    {
        Workbench.OnRepairTick -= RepairTickAudio;
        Workbench.OnRepairStart -= AudioLoopStart;
        Workbench.OnRepairStop -= AudioLoopStop;
        Workbench.OnRepairStop -= RepairFinishedAudio;
    }

    private void AudioLoopStart(Bench bench)
    {
        if (workbench != bench) return;
        loopAudio.Play();
    }

    private void AudioLoopStop(Bench bench)
    {
        if (workbench != bench) return;

        float audioLoopVolume = loopAudio.volume;
        loopAudio.DOFade(0f, 0.15f).OnComplete(() =>
        {
            loopAudio.Stop();
            loopAudio.volume = audioLoopVolume;
        });
    }

    private void RepairTickAudio(Bench bench)
    {
        if (workbench != bench) return;
        tickAudio.clip = repairTickClip;
        tickAudio.pitch = Random.Range(0.85f, 1.15f);
        tickAudio.Play();
    }

    private void RepairFinishedAudio(Bench bench)
    {
        if (workbench != bench) return;
        tickAudio.clip = repairFinishedClip;
        tickAudio.Play();
    }
}
