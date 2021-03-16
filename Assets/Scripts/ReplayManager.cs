using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReplayManager : MonoBehaviour
{
    // audio source and clip
    public AudioSource musicSource;
    public AudioClip musicClip;

    // replay car
    public ReplayController car;
    private bool isReplay = false;


    public void PlayReplay(CarTracking carTrack)
    {
        if (!isReplay)
        {
            // set the bool to true and start the replay with the track recorded in the last game
            isReplay = true;
            car.StartReplay(carTrack);

            // set the music settings
            PlayMusic();
        }
    }

    void Update()
    {
        if (isReplay)
        {
            // When player hits Esc, quit replay
            if (Input.GetButtonDown("Cancel")) 
                SceneManager.LoadScene("Intro");
        }
    }

    // method to play the music with its settings
    private void PlayMusic()
    {
        musicSource.clip = musicClip;
        musicSource.loop = true;
        musicSource.volume = 0.5f;
        musicSource.Play();
    }
}
