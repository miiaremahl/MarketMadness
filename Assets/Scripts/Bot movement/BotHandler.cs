using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
 * Class responsible for all the bots.
 * Miia Remahl 
 * mrema003@gold.ac.uk
 * last edited: 10.1.2020
 * 
 */

public class BotHandler : MonoBehaviour
{
    //all the bots
    public List<Customers.Customer> bots = new List<Customers.Customer>();

    //Start the bot movement to items
    public void StartMovement()
    {
        foreach (var bot in bots)
        {
            bot.StartMovingToItemGroup();
        }
    }

    //game is paused
    public void PauseGame()
    {
        foreach (var bot in bots)
        {
            bot.PauseGame();
        }
    }

    //resumes paused game
    public void ResumeGame()
    {
        foreach (var bot in bots)
        {
            bot.ResumeGame();
        }
    }

    //game ends
    public void EndGame()
    {
        foreach (var bot in bots)
        {
            bot.EndGame();
        }
    }
}
