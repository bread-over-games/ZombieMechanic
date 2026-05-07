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
        GameManager.OnGameStart += FirstSong;
    }

    private void OnDisable()
    {
        GameManager.OnGameStart -= FirstSong;
    }

    private void FirstSong() // plays first song at the start of the game, then it goes on loop
    {
        audioSource.clip = musicTracks[0];
        audioSource.Play();
        Invoke("StartMusicLoop", musicTracks[0].length + 20);
    }

    private void StartMusicLoop()
    {
        StartCoroutine(MusicLoop());
    }

    private IEnumerator MusicLoop()
    {
        int lastTrackIndex = 0;

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
