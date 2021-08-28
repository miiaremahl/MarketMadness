using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * Class for the alarm logic
 * Miia Remahl 
 * mrema003@gold.ac.uk
 * last edited: 25.1.2020
 * 
 * References:
 */
public class AlarmScript : MonoBehaviour
{
    //alarm
    public AudioSource alarm;
    public AudioSource alarm2;
    public AudioSource alarm3;
    public AudioSource alarm4;

    //are alarms on
    public bool enterAlarmOn = false;
    public bool exitAlarmOn = false;

    //play alarm sound
    public void PlayEntryAlarms()
    {
        if (!enterAlarmOn)
        {
            alarm.Play();
            alarm2.Play();
            enterAlarmOn = true;
            StartCoroutine(EnterAlarmTimer());
        }
    }

    //play alarm sound
    public void PlayExitAlarms()
    {
        if (!exitAlarmOn)
        {
            alarm3.Play();
            alarm4.Play();
            exitAlarmOn = true;
            StartCoroutine(ExitAlarmTimer());
        }
    }


    //coroutines for alar times
    IEnumerator ExitAlarmTimer()
    {
        yield return new WaitForSeconds(0.5f);
        exitAlarmOn = false;
    }

    IEnumerator EnterAlarmTimer()
    {
        yield return new WaitForSeconds(0.5f);
        enterAlarmOn = false;
    }

}
