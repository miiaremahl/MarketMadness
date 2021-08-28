using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Helps to check if player has collided with trigger.
 * Miia Remahl 
 * mrema003@gold.ac.uk
 * Last edited: 24.1.2021
 */


public class TriggerChecker : MonoBehaviour
{
    //cashier
    public Cashier cashier;

    //player
    public Player.PlayerBehaviour player;

    //trigger enter
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Cashier"))
        {
            cashier.HandleCashierTrigger();
        }
        else if (other.gameObject.CompareTag("EntryAlarms"))
        {
            player.CheckItemStatus("entry");
        }
        else if (other.gameObject.CompareTag("ExitAlarms"))
        {
            player.CheckItemStatus("exit");
        }
    }

    //trigger exit
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Cashier"))
        {
            cashier.HandleCashierExit();
        }
    }
}
