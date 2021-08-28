using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/*
 * Class for ItemGroup.
 * 
 * Miia Remahl 
 * mrema003@gold.ac.uk
 * Last edited: 2.1.2021
 * 
 * References:
 * 1. Sunny Valley Studio - Procedural town : https://www.youtube.com/watch?v=umedtEzrpvU&list=PLcRSafycjWFcbaI8Dzab9sTy5cAQzLHoy&index=1,
 *  took some ideas of how to make easily spawnable class and tried to modify it a bit.
 */


namespace Items
{
    [Serializable]
    public class ItemGroup
    {
        #region Procedural generation
        //refs: 1. for procedural placement

        //prefab for the itemgroup
        [SerializeField]
        private GameObject prefab;

        //Horizontal size of the itemgroup when placed
        public int horizontalSizeRequired;

        //Vertical size of the itemgroup when placed
        public int verticalSizeRequired;

        //How many this kind of groups can be placed
        public int amount;

        //how many placed already
        public int numOfPlaced;

        //is a default item
        public bool defaultItem;
        #endregion

        #region Procedural logic
        //refs: 1. for procedural placement

        //returns a prefab
        public GameObject GetPrefab()
        {
            numOfPlaced++;
            return prefab;
        }

        //are all groups placed
        public bool hasItems()
        {
            return numOfPlaced < amount;
        }

        //reset the placed amount
        public void ResetAmount()
        {
            numOfPlaced = 0;
        }

        #endregion
    }
}


