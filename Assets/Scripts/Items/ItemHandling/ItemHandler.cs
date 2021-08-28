using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
 * class for handling items related stuff.
 * 
 * Miia Remahl 
 * mrema003@gold.ac.uk
 * Last edited: 3.1.2021
 * 
 * References:
*/

namespace Items
{
    public class ItemHandler : MonoBehaviour
    {
        [Header(header: "Spawned item types")]
        //all the store itemgroups
        public ItemGroup[] itemGroups;

        [Header(header: "Object references")]
        public NavMeshUpdate navmesh; //navmesh
        public Transform player; //player
        public UI.UIHandling UI; //UI

        //selected objects
        private List<ItemGroupInstance> selectedGroups =new List<ItemGroupInstance>();

        //all the spawned objects
        private Dictionary<Vector3Int, GameObject> spawnedObjects = new Dictionary<Vector3Int, GameObject>();

        //Objects active to be selected
        private List<Vector3Int> activeToSelect;

        //items that can be picked (chosen item group)
        public List<Item> availableItems;

        //items that can be carried (chosen item group)
        public List<Item> carriedItems = new List<Item>();

        //items that are paid
        public List<Item> paidItems = new List<Item>();

        #region PRIVATE Functions


        //select random object from object list
        private void ChooseRandomObject()
        {
            if (activeToSelect.Count > 0 )
            {
                //choose random index
                System.Random ran = new System.Random();
                int index = ran.Next(activeToSelect.Count);

                //set the item to find and remove it from the list
                Vector3Int key = activeToSelect[index];
                ItemGroupInstance group  = spawnedObjects[key].GetComponent<Items.ItemGroupInstance>();
                selectedGroups.Add(group); //add item to selected
                SetAvailableItems(group); //set availbale items
                group.ActivateGroup(); //activate bot triggers
                UI.SetItemToFind(group); //tell user which was selected
                activeToSelect.RemoveAt(index); //remove from list to be selected
            }
        }

        //set available items to be picked
        private void SetAvailableItems(ItemGroupInstance group)
        {
            availableItems = new List<Item>(group.items); //make a copy
            UI.SetItemsLeft(availableItems.Count); //Set items in UI
        }

        //Item is picked
        public void ItemPicked(Item item)
        {
            carriedItems.Add(item); //no more available for picking
            availableItems.Remove(item);
        }

        //Item was paid 
        public void ItemPaid(Item item)
        {
            carriedItems.Remove(item); //is noit carried anymore
            paidItems.Add(item); //add to paid
            UI.SetItemsLeft(availableItems.Count + carriedItems.Count);
            if (availableItems.Count + carriedItems.Count <= 0) // out of items
            {
                UI.GameOverScreen("ALL ITEMS SOLD");
            }
        }

        #endregion

        #region PUBLIC Functions

        //temporary fucntion to get only one selected group -> in the future dev logic might change
        public ItemGroupInstance GetSelectedGroup()
        {
            return selectedGroups[0];
        }

        //returns selected group
        public List<ItemGroupInstance> GetSelectedGroups()
        {
            return selectedGroups;
        }

        //activated when procedural generation is done
        public void allSpawned()
        {
            //Build navmesh
            navmesh.BakeNavmesh();

            //create list for every object group that could be selected
            activeToSelect = new List<Vector3Int>(spawnedObjects.Keys);

            //choose the item to find
            ChooseRandomObject();

        }

        //return the position of the actively chosen item
        public Vector3 getItemGroupDestination()
        {
            return GetSelectedGroup().GetNavPosition(); //might change in future dev
        }

        //are there items not picked
        public bool ItemsAvailable()
        {
            return availableItems.Count > 0;
        }

        //return what type the looked items are
        public ItemData GetDestinationItemType()
        {
            return availableItems[0].itemData;
        }

        //return an item destination for the bot
        public Item getDestinationItem()
        {
            //choose random index -> random item from available to be the destination
            System.Random ran = new System.Random();
            int index = ran.Next(availableItems.Count);
            return availableItems[index];
        }

        //return itemgroups
        public ItemGroup[] GetItemGroups()
        {
            return itemGroups;
        }

        //adds object to spawned list (dictionary)
        public void AddSpawned(Vector3Int position, GameObject itemGroup)
        {
            spawnedObjects.Add(position, itemGroup);
            itemGroup.GetComponent<ItemGroupInstance>().AddItemRefs(player, UI);
        }
        #endregion
    }
}