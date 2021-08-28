using UnityEngine;

/*
 * Class for Interactable objects
 * Miia Remahl 
 * mrema003@gold.ac.uk
 * last edited: 9.1.2021
 * 
 * References:
 * 1. Brackeys - ITEMS - Making an RPG in Unity (E04) ,https://www.youtube.com/watch?v=HQNl3Ff2Lpo&list=PLPV2KyIb3jR4KLGCCAciWQ5qHudKtYeP7&index=5&t=21s
 */


public class ItemPickUp : Interactable
{
    //UI
    public UI.UIHandling UI;

    //item 
    public GameObject item;

    //override interact from interactable class
    public override void Interact()
   {
        PickUp();
        base.Interact();
   }

    //pick up the object
    private void PickUp()
    {
        gameObject.SetActive(false);
        Items.Inventory.instance.Add(item);
        UI.HidePickUp();
    }

    //add the references to item
    public void AddReferences(Transform player, GameObject item, UI.UIHandling UI)
    {
        this.UI = UI;
        this.item = item;
        this.player = player;
    }
}
