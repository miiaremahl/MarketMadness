using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/*
 * Class for setting UI things in the game
 * Miia Remahl 
 * mrema003@gold.ac.uk
 * last edited: 25.1.2021
 * 
 * References:
 * 1. John - How to make a countdown timer in Unity (in minutes + seconds) : https://gamedevbeginner.com/how-to-make-countdown-timer-in-unity-minutes-seconds/#timer, took some ideas how to display timer
 * 2. brackeys - How to make a HEALTH BAR in Unity! https://www.youtube.com/watch?v=BLfNP4Sc_iA
 */

namespace UI
{
    public class UIHandling : MonoBehaviour
    {
        #region Variables
        //ref to general logic
        public Logic.GeneralLogic generalLogic;

        //items left text
        [SerializeField] TextMeshProUGUI itemsLeft;

        //Timer text
        [SerializeField] TextMeshProUGUI timer;

        //Pausemenu object
        public GameObject pauseMenu;

        //Pickup message
        public GameObject pickUpCanvas;

        #region count down
        //Countdown text and object
        [SerializeField] TextMeshProUGUI countDown;
        public GameObject countDownObj;

        //Countdown object
        public GameObject countDownCanvas;

        #endregion

        #region check out
        //Checkout 
        public GameObject checkout;
        public TextMeshProUGUI checkoutTxt;
        public bool checkOutDisplayed = false;
        #endregion

        #region Sale add
        private Sprite displayedSaleItem; //dispayed sale item = item to find
        public GameObject saleAdd; //saleadd object
        public Image saleImage; //picture of the item
        public Image itemToFind; //item to find image
        public GameObject itemFindObj; //item to find
        public string saleItemType; // saleitem type
        public AudioSource saleSound; //sound when add comes up
        #endregion

        #region fight panel
        public GameObject fightpanel;
        public Gradient gradient;
        public Image fill;
        public Slider slider;
        public bool fightPanelDisplayed = false;
        public GameObject fightSuggestion; //player to challenge
        public bool fightSuggestionOn = false;
        #endregion

        #region Instructor

        //Instructor object
        public GameObject instructor;

        //is instructor shown
        private bool showingInst;


        #endregion

        #region End Screen
        public GameObject endScreen; //end screen
        [SerializeField] TextMeshProUGUI endTitle; //end text
        public Image endItemImage; // image for the item
        public GameObject checkmark; //checkmark
        public GameObject failmark; //failmark
        [SerializeField] TextMeshProUGUI endItem; //name of the item
        [SerializeField] TextMeshProUGUI pointText; //final points
        [SerializeField] TextMeshProUGUI itemText; //items collected
        [SerializeField] TextMeshProUGUI timeBonusText; //timebonus
        public bool gameOverActivated = false;
        #endregion

        #endregion

        #region Functions

        //The game starts
        public void StartGame()
        {
            StartCoroutine(SaleTimer());
        }

        #region Pickup
        //display pickup text
        public void DisplayPickUp()
        {
            pickUpCanvas.SetActive(true);
        }

        //hide pickup text
        public void HidePickUp()
        {
            pickUpCanvas.SetActive(false);
        }

        #endregion

        #region SALE display
        //set how many items are left in stock
        public void SetItemsLeft(float count)
        {
            itemsLeft.text = "Items in stock : " + count.ToString();
        }

        //set item to find for the user to be seen
        public void SetItemToFind(Items.ItemGroupInstance group)
        {
            displayedSaleItem = group.itemtype.image;
            saleItemType = group.itemtype.type;
            itemToFind.sprite = displayedSaleItem;
            StartGame();
        }

        //display the sale add
        private void DisplaySale()
        {
            saleSound.Play();
            saleImage.sprite = displayedSaleItem;
            saleAdd.SetActive(true);
        }

        //hide sale add
        private void HideSale()
        {
            saleAdd.SetActive(false);
        }

        //show item to find
        private void showItemToFind()
        {
            itemFindObj.SetActive(true);
        }
        #endregion

        #region Check out

        //show the checkout note
        public void DisplayCheckOutText(string text)
        {
            if (!checkOutDisplayed)
            {
                checkOutDisplayed = true;
                checkoutTxt.text = text;
                checkout.SetActive(true);
            }
        }

        //hide the checkout note
        public void HideCheckOutText()
        {
            if (checkOutDisplayed)
            {
                checkOutDisplayed = false;
                checkout.SetActive(false);
            }
        }

        #endregion

        #region instructor
        //show play guidance
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
                hideInstructor();
            }
        }

        //hide guidance
        public void hideInstructor()
        {
            showingInst = false;
            instructor.SetActive(false);
        }
        #endregion

        #region Fight panel
        //display fight panel
        public void displayFightPanel(float max, float val)
        {
            if (!fightPanelDisplayed)
            {
                fightpanel.SetActive(true);
                slider.maxValue = max;
                slider.value = val;
                fightPanelDisplayed = true;
                fill.color = gradient.Evaluate(1f);
            }

        }

        //set the bar value, refs: 2.brackeys
        public void SetFightBarVal(float val)
        {
            slider.value = val;
            fill.color = gradient.Evaluate(slider.normalizedValue);
        }

        //hide fight panel
        public void HideFightPanel()
        {
            fightpanel.SetActive(false);
            fightPanelDisplayed = false;

        }

        //Player can challenge others
        public void DisplayFightSuggestion()
        {
            if (!gameOverActivated)
            {
                fightSuggestion.SetActive(true);
                fightSuggestionOn = true;
            }
        }

        //hides the suggestion to fight others
        public void HideFightSuggestion()
        {
            fightSuggestion.SetActive(false);
            fightSuggestionOn = false;
        }

        #endregion

        #region End screen
        //activates the end screen
        public void ActiveEndScreen(bool won, float points, float items, float timebonus)
        {
            gameOverActivated = true;

            if (fightSuggestionOn)
            {
                HideFightSuggestion();
            }
            if (checkOutDisplayed)
            {
                HideCheckOutText();
            }
            if (fightPanelDisplayed)
            {
                HideFightPanel();
            }
            generalLogic.EndGame();
            endScreen.SetActive(true);
            endItemImage.sprite = displayedSaleItem; //display saleitem
            endItem.text = saleItemType; //name of the item

            if (won)
            {
                endTitle.text = "Victory";
                checkmark.SetActive(true);
                pointText.text = points.ToString() + " points"; //points
                itemText.text = items.ToString() + " item"; //item count
                timeBonusText.text = timebonus.ToString() + " timebonus"; //timebonus
                generalLogic.GameWon();
            }
            else
            {
                endTitle.text = "Game over";
                failmark.SetActive(true);
                pointText.text = 0.ToString() + " points"; //points
                itemText.text = 0.ToString() + " item"; //item count
                timeBonusText.text = 0.ToString() + " timebonus"; //timebonus
                generalLogic.GameOver();
            }
        }



        //timeout 
        public void GameOverScreen(string message)
        {
            gameOverActivated = true;
            if (fightSuggestionOn)
            {
                HideFightSuggestion();
            }
            if (checkOutDisplayed)
            {
                HideCheckOutText();
            }
            if (fightPanelDisplayed)
            {
                HideFightPanel();
            }
            generalLogic.EndGame();
            endScreen.SetActive(true);
            endItemImage.sprite = displayedSaleItem; //display saleitem
            endItem.text = saleItemType; //name of the item
            endTitle.text = message;
            failmark.SetActive(true);
            pointText.text = 0.ToString() + " points"; //points
            itemText.text = 0.ToString() + " item"; //item count
            timeBonusText.text = 0.ToString() + " timebonus"; //timebonus
            generalLogic.GameOver();
        }


        #endregion

        #region Pause Menu

        //Show pause menu
        public void DisplayPauseMenu()
        {
            Cursor.lockState = CursorLockMode.None;
            pauseMenu.SetActive(true);
        }

        //Hide pause menu
        public void HidePauseMenu()
        {
            if (showingInst)
            {
                hideInstructor();
            }
            pauseMenu.SetActive(false);
        }

        #endregion

        #region Count down
        //set count down text
        public void SetCountDown(string val)
        {
            countDown.text = val;
        }

        //disables countdown text
        public void disableCountDown()
        {
            countDownObj.SetActive(false);
        }
        #endregion

        //display timer on the screen
        //refs:1 for how to dispaly min/sec etc.
        public void DisplayTimer(float time)
        {
            float min = Mathf.FloorToInt((time + 1) / 60);
            float sec = Mathf.FloorToInt((time + 1) % 60);
            timer.text = string.Format("{0:00}:{1:00}", min, sec);
        }

        //Coroutine for sale display
        IEnumerator SaleTimer()
        {
            DisplaySale();
            yield return new WaitForSeconds(5);
            HideSale();
            showItemToFind();
            yield return new WaitForSeconds(1);
            generalLogic.AllItemsSpawned();
        }

        #endregion
    }
}
