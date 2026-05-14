using UnityEngine;
using DG.Tweening;

public class BenchConstructibleAudio : MonoBehaviour
{
    [SerializeField] private AudioSource loopAudio;
    [SerializeField] private AudioSource tickAudio;

    [SerializeField] private AudioClip constructionTickClip;
    [SerializeField] private AudioClip constructionFinishedClip;

    private void OnEnable()
    {
        BenchConstructible.OnConstructionStart += AudioLoopStart;
        BenchConstructible.OnConstructionFinished += AudioLoopStop;
        BenchConstructible.OnConstructionFinished += ConstructionFinishedAudio;
        BenchConstructible.OnConstructionStop += AudioLoopStop;
        BenchConstructible.OnConstructionTick += ConstructionTickAudio;
    }

    private void OnDisable()
    {
        BenchConstructible.OnConstructionStart -= AudioLoopStart;
        BenchConstructible.OnConstructionFinished -= AudioLoopStop;
        BenchConstructible.OnConstructionFinished -= ConstructionFinishedAudio;
        BenchConstructible.OnConstructionStop -= AudioLoopStop;
        BenchConstructible.OnConstructionTick -= ConstructionTickAudio;
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

    private void ConstructionTickAudio(GameObject obj)
    {
        if (obj != gameObject) return;
        tickAudio.clip = constructionTickClip;
        tickAudio.pitch = Random.Range(0.85f, 1.15f);
        tickAudio.Play();
    }

    private void ConstructionFinishedAudio(GameObject obj)
    {
        if (obj != gameObject) return;
        AudioSource.PlayClipAtPoint(constructionFinishedClip, transform.position, 0.5f);
    }
}
