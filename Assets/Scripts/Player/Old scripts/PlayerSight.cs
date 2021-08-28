using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * OLD SCRIPT FOR SEEING.
 * Miia Remahl 
 * mrema003@gold.ac.uk
 * last edited: 10.1.2020 -> REMOVED FROM USE
 * 
 * References:
 * 1. Brackeys - FIRST PERSON MOVEMENT in Unity - FPS Controller : Used this for changing the vertical view of which character is seeing
 */

namespace Player
{
    public class PlayerSight : MonoBehaviour
    {
        //#region OLDCode
        //#region PUBLIC FIELDS
        ////Mouse sensitivity
        //public float sensitivity = 100f;
        //#endregion

        //#region PRIVATE FIELDS
        //// X rotation
        //float xRotation = 0f;

        ////is game paused
        //private bool gamePaused = false;
        //#endregion

        //#region Setup
        //void Start()
        //{
        //    //lock the cursor
        //    Cursor.lockState = CursorLockMode.Locked;
        //}
        //#endregion

        //#region Update functions
        //void Update()
        //{
        //    if (!gamePaused)
        //    {
        //        ////get mouse movement
        //        //float mouseY = Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;

        //        //xRotation -= mouseY;

        //        ////clamp the rotation
        //        //xRotation = Mathf.Clamp(xRotation, -90, 90);

        //        ////change the vertical view
        //        //transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        //    }
        //}
        //#endregion

        //#region Pausing
        ////pause game
        //public void Pause()
        //{
        //    gamePaused = true;
        //}

        ////continue game
        //public void Continue()
        //{
        //    gamePaused = false;
        //    Cursor.lockState = CursorLockMode.Locked;
        //}
        //#endregion
        //#endregion
    }

}
