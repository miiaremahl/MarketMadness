using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
 * Class for the main door of the store.
 * Miia Remahl 
 * mrema003@gold.ac.uk
 * last edited: 20.12.2020
 * 
 * References:
 */
namespace Store
{
    public class Door : MonoBehaviour
    {
        //door animation
        public Animator door;

        //sound for opening door
        public AudioSource opening;

        //Open the door of the store
        public void OpenDoor()
        {
            if (opening)
            {
                opening.Play();
            }
            door.SetBool("DoorOpening", true);
        }
    }
}
