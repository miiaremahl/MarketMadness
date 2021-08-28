using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Player behaviour class.
 * Miia Remahl 
 * mrema003@gold.ac.uk
 * Last edited: 24.1.2021
 * 
 * References:
 * 1. Jayanam - Unity Inventory Tutorial : Pickup on Keypress & Trigger : https://www.youtube.com/watch?v=90OiysC4j5Y&list=PLboXykqtm8dynMisqs4_oKvAIZedixvtf&index=11, got some ideas on how to make the item picking
 * 2. Brackeys - INTERACTION - Making an RPG in Unity (E02) :https://www.youtube.com/watch?v=9tePzyL6dgc&list=PLPV2KyIb3jR4KLGCCAciWQ5qHudKtYeP7&index=3 : took example for the interactable part. 
 * 3. hathol - how to get component of the raycast hit https://answers.unity.com/questions/262012/how-to-get-component-of-the-raycast-hit.html, for getting the component that we hit
 * 4. UnityDocumentation - Raycast :https://docs.unity3d.com/ScriptReference/Physics.Raycast.html / https://docs.unity3d.com/ScriptReference/RaycastHit-distance.html , determining the hit distance 
 * 5. Jayanam - Unity Inventory System : Use Items :https://www.youtube.com/watch?v=Uk91lEiKn2g&list=PLboXykqtm8dynMisqs4_oKvAIZedixvtf&index=7, take some ideas how to display the carried object
 */

namespace Player
{
    public class PlayerBehaviour : MonoBehaviour
    {
        #region Variables 
        //alarms
        public AlarmScript alarms;

        //is game paused
        private bool gamePaused = false;

        //is game over
        private bool gameOver = false;

        [Header(header: "Object references")]
        //UI
        public UI.UIHandling UI;

        #region player body
        //fps controller
        public UnityStandardAssets.Characters.FirstPerson.FirstPersonController fpsController;

        //orientation of player
        public Transform orientation;

        //reference to hand
        public GameObject handSmallItem;
        public GameObject handBigItem;

        //animation helper
        public PlayerAnimation animationHandler;

        #endregion

        #region fighting
        //is someone chasing the player?
        public int isChased = 0;

        //is player fighting
        public bool isFighting = false;

        //how much to win the fight
        private float fightWon = 10f;
        private float fightVal = 5f;

        //is fighting for others item
        private bool startedFight = false;

        //who player is fighting with
        public Customers.Customer fightingWith;

        //fight text is shown/not
        bool fightModeDisplayed = false;
        #endregion

        #region OLD code
        //ref to player movement scripts
        // public Player.RigidBodyMovement playerMovement;
        #endregion

        #region Item picking
        [Header(header: "Raycast")]
        //How long distance do we count for the raycast to hit object
        public float maxRayHitDist = 3f;

        //what we are focusing on 
        private Interactable focus;

        //are we carrying an item
        public bool carryingItem = false;

        //carried item
        public Items.Item itemCarried;

        //inventory
        public Items.Inventory inventory;

        //item type that player is looking for 
        public ItemData lookedItemType;
        #endregion

        //Item selector reference
        public Items.ItemHandler itemHandler;

        //Is UI text for pick up set to display
        private bool pickUpDisplayed = false;

        #region Audio

        //punching
        public AudioSource fight;
        //unpleased
        public AudioSource unpleased;
        //won the fight sound
        public AudioSource happy;

        #endregion

        #endregion

        public void Update()
        {
            if (!gameOver)
            {
                //Refs: 2. Brackeys , used this to make the raycast interaction, however had to modify it heavily since the code didnt work as in tutorial
                // 3.hathol , for getting component thats been hit, 4.UnityDocumentation raycast usage and hit distance
                if (!gamePaused && !isFighting)
                {
                    RaycastHit hit;
                    if (Physics.SphereCast(orientation.transform.position, 1, orientation.transform.TransformDirection(Vector3.forward), out hit, 1)) //is SphereCast hitting something
                    {
                        if (!carryingItem) //only if we are not carrying anything
                        {
                            Interactable interactable = hit.transform.gameObject.GetComponent<ItemPickUp>();
                            if (hit.distance < maxRayHitDist && interactable != null) //is the hit within the distance and interactable
                            {
                                Items.Item item = interactable.GetComponent<Items.Item>();
                                if (lookedItemType.type == item.itemData.type) //right item
                                {
                                    if (!pickUpDisplayed)
                                    {
                                        UI.DisplayPickUp(); //display pickup text
                                        pickUpDisplayed = true;
                                    }
                                    if (Input.GetMouseButtonDown(0)) //is mouse left pressed
                                    {
                                        SetFocus(interactable); //set focus to intectable object
                                    }
                                }
                            }
                            else if (pickUpDisplayed)
                            {
                                HidePickUp();
                            }

                            //fighting w others
                            if(hit.distance < maxRayHitDist && interactable == null)
                            {
                                CheckIfHitCustomer(hit); //are we hitting customer with SphereCast 
                            }
                            else if (fightModeDisplayed)
                            {
                                fightModeDisplayed = false;
                                UI.HideFightSuggestion();
                            }
                        }
                    }
                    else if (pickUpDisplayed || fightModeDisplayed)
                    {
                        if (pickUpDisplayed)
                        {
                            HidePickUp();
                        }
                        if (fightModeDisplayed)
                        {
                            fightModeDisplayed = false;
                            UI.HideFightSuggestion();
                        }
                    }
                    #region OLD Code
                    //refs 1: Jayanam - item picking notes
                    //if (itemToPick!= null && Input.GetKeyDown(KeyCode.E))
                    //{
                    //    PickUpItem();

                    //    UI.HidePickUp();
                    //    itemToPick = null;
                    //}
                    #endregion
                }
                else if (!gamePaused && isFighting) //Fighting
                { 
                    //If fight key is pressed increase chance of winning
                    if (Input.GetKeyDown(KeyCode.Return))
                    {
                        fightVal += 1;
                        UI.SetFightBarVal(fightVal);
                    }

                    //check fight status
                    if (fightVal >= fightWon)
                    {
                        EndFight(true);
                    }
                    else if (fightVal <= 0)
                    {
                        EndFight(false);
                    }
                }
            }
        }

        //called when game starts
        public void GameStarted()
        {
            lookedItemType = itemHandler.GetDestinationItemType(); //get what type item looking for
        }

        //continue game
        public void ContinueGame()
        {
            gamePaused = false;
            fpsController.ContinueGame();
        }

        //pause game
        public void PauseGame()
        {
            gamePaused = true;
            fpsController.PauseGame();
        }

        //someone is chasing player
        public void BeingChased()
        {
            isChased++;
        }

        //chasing stopped
        public void NotChasedAnyMore()
        {
            isChased = isChased > 0 ? isChased-1 : 0;
        }

        //end game
        public void EndGame()
        {
            gameOver = true;
            fpsController.EndGame();
        }

        //check if player has item -> if has set alarms
        public void CheckItemStatus(string alarmType)
        {
            if (carryingItem)
            {
                if (alarmType == "entry")
                {
                    alarms.PlayEntryAlarms();
                }
                else
                {
                    alarms.PlayExitAlarms();
                }
                
            }
        }

        //get into a fight
        public void StartFighting(Customers.Customer customer)
        {
            fight.Play();
            fightVal = fightWon / 2; //set fightval
            UI.displayFightPanel(fightWon, fightVal);
            isFighting = true;
            fightingWith = customer;
            fpsController.PauseGame(); //pause movement
            StartCoroutine(FightCountDown()); //start decreasing fightval
        }

        //end fight
        public void EndFight(bool won)
        {
            animationHandler.EndFight(); //animation
            fight.Pause(); //audio
            UI.HideFightPanel(); //UI change
            isFighting = false;
            fpsController.ContinueGame(); //free to move
            if (won == false)
            {
                unpleased.Play(); //audio
                animationHandler.LoseItem();
                if (startedFight) //if the player started the fight
                {
                    fightingWith.FightWonWhenChallenged();
                    fightModeDisplayed = false;
                    startedFight = false;
                    UI.HideFightSuggestion();
                }
                else
                {
                    carryingItem = false;
                    inventory.Remove(itemCarried.gameObject); //lose the item
                    fightingWith.FightWon(itemCarried.gameObject.GetComponent<ItemPickUp>()); //customer wins
                }   
            }
            else{
                happy.Play();
                if (startedFight) //if the player started the fight
                {
                    fightModeDisplayed = false;
                    UI.HideFightSuggestion();
                    startedFight = false;
                    Items.Item item= fightingWith.LoseItem();
                    if (item)
                    {
                        SetFocus(item.GetComponent<ItemPickUp>());
                    }
                }
                fightingWith.FightLost(); //customer loses
            }
            fightingWith = null;
        }

        //Carry an item (show graphics) refs. 5. Jayanam: took an idea on how to display the item carried
        public void CarryItem(GameObject item)
        {
            itemCarried = item.GetComponent<Items.Item>();
            itemHandler.ItemPicked(itemCarried); //remove from free to pick list
            itemCarried.carried = true; //set the item to be carried
            carryingItem = true;
            item.SetActive(true);

            if (itemCarried.itemData.itemSize == "small") //small items
            {
                animationHandler.PickUpSmall();
                item.transform.parent = handSmallItem.transform;
                item.transform.position = handSmallItem.transform.position;
            }
            else if(itemCarried.itemData.itemSize == "big") //big items
            {
                animationHandler.PickUpBig();
                item.transform.parent = handBigItem.transform;
                item.transform.position = handBigItem.transform.position;
            }
        }

        //hides the pickup text
        private void HidePickUp()
        {
            pickUpDisplayed = false;
            UI.HidePickUp();
        }

        //Sets the focus to given interactable : ref.2 Brackeys
        void SetFocus(Interactable newFocus)
        {
            if (newFocus != focus)
            {
                if (focus != null)
                {
                    focus.DeFocused();
                }
                focus = newFocus;
            }
            newFocus.Focused(transform);
            pickUpDisplayed = false;
        }

        //helper for fight 
        IEnumerator FightCountDown()
        {
            while (isFighting)
            {
                yield return new WaitForSeconds(0.3f);
                fightVal -= 1;
                UI.SetFightBarVal(fightVal);
            }
        }

        //handles checking if we can fight w other customers
        public void CheckIfHitCustomer(RaycastHit hit)
        {
            Customers.Customer customer = hit.transform.gameObject.GetComponent<Customers.Customer>();
            if (customer != null) //did we hit customer
            {
                if (!fightModeDisplayed && customer.carryingItem) //do they have an item 
                {
                    fightModeDisplayed = true;
                    UI.DisplayFightSuggestion(); //display fight suggestion
                }

                if (fightModeDisplayed && customer.carryingItem)
                {
                    if (Input.GetMouseButtonDown(0)) //is mouse left pressed
                    {
                        animationHandler.Punch(); //start fight
                        fightModeDisplayed = false;
                        customer.GetIntoFight(orientation);
                        StartFighting(customer);
                        startedFight = true;
                    }
                }
            }
        }
        #region OLD Code
        //temp storing of the pickable item
        //private Items.Item itemToPick = null;

        //currently picked item
        //private Items.Item itemPicked = null;

        ////refs 1.Jayanam, got some idea on how to do the item picking -> changed the code a lot

        ////enter trigger 
        //private void OnTriggerEnter(Collider collider)
        //{
        //    if (collider.gameObject.CompareTag("item")){ //did we collide with item

        //        Items.Item item = collider.transform.parent.gameObject.GetComponent<Items.Item>(); //get parents item class
        //        if (item != null) //double check there is item class
        //        {
        //            UI.DisplayPickUp(); //show text

        //            itemToPick = item; //temporarily store the item
        //        }

        //    }

        //}

        ////exit trigger
        //private void OnTriggerExit(Collider collider)
        //{
        //    if (collider.gameObject.CompareTag("item"))
        //    {
        //        UI.HidePickUp();
        //        itemToPick = null;
        //    }
        //}

        //picking up item
        //public void PickUpItem()
        //{
        //    ïtemPicked = itemToPick;
        //}
        #endregion

    }
}
