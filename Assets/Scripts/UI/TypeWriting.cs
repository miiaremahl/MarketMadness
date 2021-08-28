using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


/*
 * TypeWriting class. Calls the TypeWriter class.
 * For skipping text: press enter.
 * Miia Remahl 
 * mrema003@gold.ac.uk
 * last edited: 4.1.2021
 * 
 * References:
 * 1. Code Monkey - How to make Text Writing Effect in Unity, https://www.youtube.com/watch?v=ZVh4nH8Mayg :
 * took ideas for the type writing effect and tried to make it work for my game.
 */

public class TypeWriting : MonoBehaviour
{
    #region PUBLIC VARIABLES
    [Header(header: "INSTRUCTION TEXTS")]
    //stored messages for the writer
    public string[] messages;

    [Header(header: "Object references")]
    //Instruction text
    public TextMeshProUGUI instructionText;

    //reference to UI
    public UI.UIHandling UI;

    //reference to menuUI
    public UI.MenuUI MenuUI;

    //audiosource for the audio of the instructor
    public AudioSource talking;

    #endregion


    #region PRIVATE VARIABLES

    //index of the display message
    private int msgIndex=0;

    //instance of a typewriter
    private TypeWriter.TypeWriterSingle typeWriterSingle;

    #endregion


    #region START/UPDATE
    //refs:1.Code Monkey - ideas for the implementation
    public void Update()
    {
        //Go to next message / display full message
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (typeWriterSingle != null && typeWriterSingle.IsActive())
            {
                typeWriterSingle.WriteAll();
            }
            else
            {
                talking.Play();
                NextMessage();
            }
        }
    }

    #endregion

    //starts the writer
    public void StartWriter()
    {
        //set start values
        msgIndex = 0;

        //play audio and display next message
        talking.Play();
        NextMessage();
    }


    #region Private functions
    //refs:1.Code Monkey : took ideas for the implementation

    //display next message
    private void NextMessage()
    {
        if(msgIndex < messages.Length)
        {
            string message = messages[msgIndex];
            msgIndex++;

            //call the typewriter instance
            typeWriterSingle = TypeWriter.AddWriter_Static(instructionText, message, 0.1f, true, StopTalking);
        }
        else
        {
            EndInstructions();
        }
    }

    //Stop the talking audio once message is displayed
    private void StopTalking()
    {
        talking.Stop();
    }

    //hides the instructor and starts the timer again
    private void EndInstructions()
    {
        if (UI != null)
        {
            UI.hideInstructor();
        }
        else
        {
            MenuUI.HideInstructor();
        }
    }

    #endregion
}
