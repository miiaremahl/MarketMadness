using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/*
 * Navigation between different scenes.
 * Miia Remahl 
 * mrema003@gold.ac.uk
 * last edited: 4.1.2021
 * 
 * References:
 */

namespace Navigation
{
    public class SceneNavigation : MonoBehaviour
    {
        //changes the scene to the gaming scene
        public void LoadGameScene()
        {
            SceneManager.LoadScene("Store");
        }

        //changes scene to main menu
        public void loadMenu()
        {
            SceneManager.LoadScene("MainMenu");
        }

    }
}
