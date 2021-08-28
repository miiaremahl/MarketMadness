using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * Class for setting UI in main menu
 * Miia Remahl 
 * mrema003@gold.ac.uk
 * last edited: 10.1.2021
 * 
 * References:
*/
namespace UI
{
    public class MenuUI : MonoBehaviour
    {
        //Instructor object
        public GameObject instructor;

        //is instructor shown
        private bool showingInst;

        //how how to play
        public void DisplayInstructor()
        {
            if (!showingInst)
            {
                instructor.SetActive(true);
                instructor.GetComponent<TypeWriting>().StartWriter();
                showingInst = true;
            }
            else
            {
                HideInstructor();
            }
        }

        //hide how to play
        public void HideInstructor()
        {
            showingInst = false;
            instructor.SetActive(false);
        }
    }
}
