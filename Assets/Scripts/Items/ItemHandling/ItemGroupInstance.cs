using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Class for ItemGroup instance.
 * 
 * Miia Remahl 
 * mrema003@gold.ac.uk
 * Last edited: 8.1.2021
 */

namespace Items
{
    public class ItemGroupInstance : MonoBehaviour
    {
        //Navigation position when navigating for the group
        public Transform[] navPositions;

        //Type of the items
        public ItemData itemtype;

        //reference to triggers object
        public GameObject triggers;

        //reference to pointers object
        public GameObject pointers;

        [Header(header: "Sale items")]
        //items in the item group
        public List<Item> items = new List<Item>();


        //add item references to each item
        public void AddItemRefs(Transform player, UI.UIHandling UI)
        {
            // add references to each item
            foreach (var item in items) 
            {
                item.GetComponent<ItemPickUp>().AddReferences(player, item.gameObject, UI);
            }
        }

        //returns the navigating position for the group
        public Vector3 GetNavPosition()
        {
            int index = UnityEngine.Random.Range(0, navPositions.Length);
            return navPositions[index].position;
        }

        //active triggers and arrows
        public void ActivateGroup()
        {
            triggers.SetActive(true);
            pointers.SetActive(true);
        }
    }
}
