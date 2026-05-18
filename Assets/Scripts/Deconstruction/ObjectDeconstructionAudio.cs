using UnityEngine;
using DG.Tweening;

public class ObjectDeconstructionAudio : MonoBehaviour
{    
    [SerializeField] private AudioSource loopAudio;
    [SerializeField] private AudioSource tickAudio;

    [SerializeField] private AudioClip deconstructionTickClip;
    [SerializeField] private AudioClip deconstructionFinishedClip;

    private void OnEnable()
    {
        ObjectDeconstruction.OnDeconstructionStart += AudioLoopStart;
        ObjectDeconstruction.OnDeconstructionFinished += AudioLoopStop;
        ObjectDeconstruction.OnDeconstructionFinished += DeconstructionFinishedAudio;
        ObjectDeconstruction.OnDeconstructionStop += AudioLoopStop;
        ObjectDeconstruction.OnDeconstructionTick += DeconstructionTickAudio;
    }

    private void OnDisable()
    {
        ObjectDeconstruction.OnDeconstructionStart -= AudioLoopStart;
        ObjectDeconstruction.OnDeconstructionFinished -= AudioLoopStop;
        ObjectDeconstruction.OnDeconstructionFinished -= DeconstructionFinishedAudio;
        ObjectDeconstruction.OnDeconstructionStop -= AudioLoopStop;
        ObjectDeconstruction.OnDeconstructionTick -= DeconstructionTickAudio;
    }

    private void AudioLoopStart(GameObject obj)
    {
        if (obj != gameObject) return;
        loopAudio.Play();
    }

    private void AudioLoopStop(GameObject obj)
    {
        if (obj != gameObject) return;
        float audioLoopVolume = loopAudio.volume;
        loopAudio.DOFade(0f, 0.15f).OnComplete(() =>
        {
            loopAudio.Stop();
            loopAudio.volume = audioLoopVolume;
        });
    }

    private void DeconstructionTickAudio(GameObject obj)
    {
        if (obj != gameObject) return;
        tickAudio.clip = deconstructionTickClip;
        tickAudio.pitch = Random.Range(0.85f, 1.15f);
        tickAudio.Play();
    }

    private void DeconstructionFinishedAudio(GameObject obj)
    {
        if (obj != gameObject) return;
        tickAudio.clip = deconstructionFinishedClip;
        tickAudio.Play();
    }
}
