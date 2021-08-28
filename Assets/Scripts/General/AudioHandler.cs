using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Helper for audio handling.
 * Miia Remahl 
 * mrema003@gold.ac.uk
 * last edited: 24.1.2020
 * 
 * References:
 */

public class AudioHandler : MonoBehaviour
{

    //theme song
    public AudioSource theme;

    //countdown 
    public AudioSource countdown;

    //gameover
    public AudioSource gameOver;

    //game won
    public AudioSource gameWon;

    //game won wohoo
    public AudioSource wohoo;

    //pause theme audio
    public void PauseThemeAudio()
    {
        theme.Pause();
    }

    //resume theme audio
    public void ResumeThemeAudio()
    {
        theme.UnPause();
    }

    //start countdown 
    public void CountDownStart()
    {
        countdown.Play();
    }

    //play Gameover sound
    public void PlayGameOverSound()
    {
        gameOver.Play();
    }

    //play game won sound
    public void PlayGameWon()
    {
        gameWon.Play();
        wohoo.Play();
    }

}
