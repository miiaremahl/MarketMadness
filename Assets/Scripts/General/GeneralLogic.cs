using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * General logic of the game
 * Miia Remahl 
 * mrema003@gold.ac.uk
 * last edited: 24.1.2021
 * 
 * References:
 */

namespace Logic
{
    public class GeneralLogic : MonoBehaviour
    {
        //Scene manager
        public Navigation.SceneNavigation sceneManager;

        //game started
        private bool gameStarted = false;

        //all items spawned and set
        private bool itemsSpawned = false;

        //has game ended
        private bool gameEnded = false;

        //handles bots
        public BotHandler botHandler;

        //UI text setter class
        public UI.UIHandling UI;

        //door class
        public Store.Door door;
        public Store.Door door2;

        //ref to player movement script
        public Player.PlayerBehaviour playerBehav;

        //timer
        private bool timerOn = false;
        public float timer = 0;

        //audio
        public AudioHandler audioHandler;

        //cursor locked at the start
        private bool cursorLocked;


        void Start()
        {
            Cursor.lockState = CursorLockMode.Locked; //lock the cursor at the start
        }

        void Update()
        {

            if (!cursorLocked) //make sure cursor is locked at the start of the game (needed this for webbuild for some reason)
            {
                cursorLocked = true;
                Cursor.lockState = CursorLockMode.Locked;
            }

            //start game
            if (!gameStarted && itemsSpawned)
            {
                gameStarted = true;
                StartCoroutine(CountDownTimer()); //count down
            }

            if (!gameEnded && gameStarted)
            {
                if (timer > 180)
                {
                    UI.GameOverScreen("OUT OF TIME");
                    EndGame();
                }

                if (timerOn)
                {
                    //change timer value
                    timer += Time.deltaTime;
                    UI.DisplayTimer(timer);
                }

                //If esc is pushed pause the game
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    PauseGame();
                }
            }
        }
    
        //all items are spawned and one is picked to be saled
        public void AllItemsSpawned()
        {
            itemsSpawned = true;
        }

        //starts the game
        private void StartGame()
        {
            playerBehav.GameStarted();
            TimerOn();
            openDoors();
        }

        #region timer/time
        //set timer on
        private void TimerOn()
        {
            timerOn = true;
        }

        //set timer off
        private void TimerOff()
        {
            timerOn = false;
        }

        //continues timing
        public void ContinueTiming()
        {
            TimerOn();
        }

        //return the timer value
        public float getTime()
        {
            return timer;
        }
        #endregion

        #region game state
        //pauses the game
        public void PauseGame()
        {
            Cursor.lockState = CursorLockMode.None;
            TimerOff();
            UI.DisplayPauseMenu();
            playerBehav.PauseGame();
            botHandler.PauseGame();
            audioHandler.PauseThemeAudio();
        }

        //resume the game after pausing
        public void ResumeGame()
        {
            Cursor.lockState = CursorLockMode.Locked;
            UI.HidePauseMenu();
            ContinueTiming();
            playerBehav.ContinueGame();
            botHandler.ResumeGame();
            audioHandler.ResumeThemeAudio();
        }

        //end the general logic of the game
        public void EndGame()
        {
            Cursor.lockState = CursorLockMode.None;
            timerOn = false;
            gameEnded = true;
            playerBehav.EndGame();
            botHandler.EndGame();
            TimerOff();
        }
        #endregion

        //restarts the game
        public void RestartGame()
        {
            sceneManager.LoadGameScene();
        }

        //start door openiong animation
        public void openDoors()
        {
            door.OpenDoor();
            door2.OpenDoor();
        }
     
        //Coroutine for the start count down
        IEnumerator CountDownTimer()
        {
            audioHandler.CountDownStart();
            UI.SetCountDown("3");
            yield return new WaitForSeconds(1);
            UI.SetCountDown("2");
            yield return new WaitForSeconds(1);
            UI.SetCountDown("1");
            yield return new WaitForSeconds(1);
            UI.SetCountDown("Start shopping!");
            StartGame();
            yield return new WaitForSeconds(1);
            botHandler.StartMovement();
            yield return new WaitForSeconds(1);
            UI.disableCountDown();
        }

        //player lost the game
        public void GameOver()
        {
            audioHandler.PlayGameOverSound();
        }

        //player won the game
        public void GameWon()
        {
            audioHandler.PlayGameWon();
        }
    }
}