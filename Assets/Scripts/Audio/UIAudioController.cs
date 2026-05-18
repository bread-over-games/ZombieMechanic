using UnityEngine;

public class UIAudioController : MonoBehaviour
{
    public static UIAudioController Instance;

    [SerializeField] private AudioSource uiSoundsAudio;
    [SerializeField] private AudioClip hoverClip;
    [SerializeField] private AudioClip clickClip;

    private void Awake()
    {
        Instance = this;
    }

    public void PlayHover()
    {
        uiSoundsAudio.clip = hoverClip;
        uiSoundsAudio.Play();
    }

    public void PlayClick()
    {
        uiSoundsAudio.clip = clickClip;
        uiSoundsAudio.Play();
    }
}
