using UnityEngine;

public class PlayerFootstepsAudio : MonoBehaviour
{
    [SerializeField] private AudioSource footstepsAudio;
    [SerializeField] private AudioClip leftFootstepClip; // boot one
    [SerializeField] private AudioClip rightFootstepClip; // metal one

    public void LeftFootStep()
    {
        footstepsAudio.clip = leftFootstepClip;
        footstepsAudio.pitch = Random.Range(0.8f, 1.05f);
        footstepsAudio.Play();
    }

    public void RightFootStep()
    {
        footstepsAudio.clip = rightFootstepClip;
        footstepsAudio.pitch = Random.Range(0.8f, 1.05f);
        footstepsAudio.Play();
    }
}
