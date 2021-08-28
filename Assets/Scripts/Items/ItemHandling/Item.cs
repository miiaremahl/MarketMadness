using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Item class.
 * Miia Remahl 
 * mrema003@gold.ac.uk
 * last edited: 10.1.2021
 * 
 * References:
 */

namespace Items
{
    public class Item : MonoBehaviour
    {
        //item data for the item
        public ItemData itemData;

        //is item pickedup
        public bool carried = false;

    }
}
