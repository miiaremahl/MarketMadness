using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


/*
 * Cashier class. In charge of ending the game.
 * 
 * Miia Remahl 
 * mrema003@gold.ac.uk
 * Last edited: 10.1.2021
 * 
 * References:
 */

public class Cashier : MonoBehaviour
{
    #region Varibles

    //ref to itemhandler
    public Items.ItemHandler itemHandler;

    //inventory
    public Items.Inventory inventory;

    //UI
    public UI.UIHandling UI;

    //general logic
    public Logic.GeneralLogic generalLogic;

    //are we showing the cashier note
    private bool cashierNoteActive = false;

    //point value of one item
    public float itemVal = 30f;

    //max bonusvalue -> time when u can get bonus points
    public float bonusTimeMax = 60f;

    //value of bonus point
    public float bonusTimeVal = 5f;

    //sound for hello
    public AudioSource hello;

    //dollar signs/audio
    public GameObject dollar1;
    public GameObject dollar2;
    public AudioSource dollarAudio;

    //is player checkout
    public bool checkedOut=false;

    #endregion

    //handles cashier trigger entries
    public void HandleCashierTrigger()
    {
        if (inventory.GetItemCount() > 0)
        {
            List<Items.ItemGroupInstance> itemsRequired = itemHandler.GetSelectedGroups(); //list of required items
            List<Items.Item> compeletedChecks = new List<Items.Item>(); //checkout complete

            //count how many items very successfully checked out
            foreach (var group in itemsRequired)
            {
                foreach (var item in inventory.getItems())
                {
                    if (group.itemtype.type == item.GetComponent<Items.Item>().itemData.type) {
                        compeletedChecks.Add(item.GetComponent<Items.Item>());
                    }
                }
            }

            FinnishGame(compeletedChecks, itemsRequired);
        }
        else
        {
            hello.Play(); //audio
            //tell to find the item
            string note = "You have no items to checkout! Find the given item!";
            UI.DisplayCheckOutText(note);
            cashierNoteActive = true;
        }
    }

    //handles cashier trigger exits
    public void HandleCashierExit()
    {
        if (cashierNoteActive)
        {
            UI.HideCheckOutText();
            cashierNoteActive = false;
        }
    }

    //game ends
    public void FinnishGame(List<Items.Item> completed, List<Items.ItemGroupInstance> required)
    {
        if (!checkedOut)
        {
            checkedOut = true;
            float timer = generalLogic.getTime(); //time
            float bonus = (float)Math.Round((bonusTimeMax - timer) * bonusTimeVal); //bonustime points
            bonus = bonus > 0 ? bonus : 0;
            float finalpoints = itemVal * completed.Count + bonus; //final points 

            bool won = completed.Count == required.Count ? true : false;
            if (won)
            {
                dollar1.SetActive(true);
                dollar2.SetActive(true);
                dollarAudio.Play();
                StartCoroutine(WinTimer(won, finalpoints, completed.Count, bonus));
            }
            else
            {
                //activate end screen (won/not, points, item count, timebonus)
                UI.ActiveEndScreen(won, finalpoints, completed.Count, bonus);
            }
        }
        
    }

    //timer before winning
    IEnumerator WinTimer(bool won, float points, float items, float timebonus)
    {
        yield return new WaitForSeconds(0.5f);
        //activate end screen (won/not, points, item count, timebonus)
        UI.ActiveEndScreen(won, points, items, timebonus);
    }
}
