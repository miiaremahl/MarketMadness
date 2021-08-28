using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Inventory class.
 * Miia Remahl 
 * mrema003@gold.ac.uk
 * last edited: 10.1.2021
 * 
 * References:
 * 1. Brackeys - ITEMS - Making an RPG in Unity (E04) ,https://www.youtube.com/watch?v=HQNl3Ff2Lpo&list=PLPV2KyIb3jR4KLGCCAciWQ5qHudKtYeP7&index=5&t=21s
 */

namespace Items
{
    public class Inventory : MonoBehaviour
    {
        //inventory
        public static Inventory instance;

        //ref to player
        public Player.PlayerBehaviour player;

        #region Singleton
        void Awake()
        {
            instance = this;
        }
        #endregion

        //all the inventory items
        public List<GameObject> items = new List<GameObject>();

        //add an item
        public void Add(GameObject item)
        {
            items.Add(item);
            SetCarriedItem();
        }

        //return how many items the inventory has
        public int GetItemCount()
        {
            return items.Count;
        }

        //return invemntory items
        public List<GameObject> getItems()
        {
            return items;
        }

        //remove the item
        public void Remove(GameObject item)
        {
            items.Remove(item);
        }

        //set carried item
        private void SetCarriedItem()
        {
            player.CarryItem(items[items.Count-1]);
        }

    }
}
