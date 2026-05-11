using UnityEngine;
using System.Collections;

public class MusicController : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip[] musicTracks;
    public float pauseMin;
    public float pauseMax;

    private void OnEnable()
    {
        GameManager.OnGameStart += StartMusicLoop;
    }

    private void OnDisable()
    {
        GameManager.OnGameStart -= StartMusicLoop;
    }

    private void StartMusicLoop()
    {
        StartCoroutine(MusicLoop());
    }

    private IEnumerator MusicLoop()
    {
        int lastTrackIndex = -1;

        while (true)
        {
            int trackIndex;

            do 
            { 
                trackIndex = Random.Range(0, musicTracks.Length); 
            }
            while (trackIndex == lastTrackIndex); // picks random track index, rerolls if it's the same as the last time

            lastTrackIndex = trackIndex;
            audioSource.clip = musicTracks[trackIndex];
            audioSource.Play();

            yield return new WaitForSeconds(musicTracks[trackIndex].length);
            yield return new WaitForSeconds(Random.Range(pauseMin, pauseMax));
        }
    }
}
