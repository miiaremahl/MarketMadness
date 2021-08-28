using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/*
 * Item group helper class: for spawning the item groups
 * 
 * Miia Remahl 
 * mrema003@gold.ac.uk
 * Last edited: 2.1.2020
 * 
 * References:
 * 1. Sunny Valley Studio - Procedural town : https://www.youtube.com/watch?v=umedtEzrpvU&list=PLcRSafycjWFcbaI8Dzab9sTy5cAQzLHoy&index=1,
 * for creating the procedural base, made some changes to fit the game.
 * The tutorial makes a town but I used it to make the market.
 * 
 * 2. Randomize a List<T> :https://stackoverflow.com/questions/273313/randomize-a-listt : for randomizing the order of the list.
 */



namespace ProceduralLogic
{
    public class ItemGroupHelper : MonoBehaviour
    {
        #region variables
        //Itemhandler: to get the list of spawnable items
        public Items.ItemHandler itemHandler;

        //Aisle helper ref
        public AisleHelper aisleHelper;

        //Blocked positions (by big item groups)
        private List<Vector3Int> blockedPositions;

        //where aisles are
        private List<Vector3Int> aislePlaces;

        //Dictionary for the free spots
        private Dictionary<Vector3Int, Direction> freeItemSpots;

        //Spawned items
        private Dictionary<Vector3Int, GameObject> spawnedGroups = new Dictionary<Vector3Int, GameObject>();
        #endregion

        #region Functions

        //places items to the store, ref:1, Sunny Valley Studio, took inspiration for the code
        public void PlaceItemGroups(List<Vector3Int> aisles)
        {
            aislePlaces = aisles; //store aisles

            //set blocked to new list and find free spots
            blockedPositions = new List<Vector3Int>();
            freeItemSpots = FindFreeSpots(aisles);

            //Get the itemgroups to spawn
            Items.ItemGroup[] itemGroups = itemHandler.GetItemGroups();

            //create a list of keys in freespots
            List<Vector3Int> keys = new List<Vector3Int>(freeItemSpots.Keys);

            //randomize order
            keys = Randomize(keys);

            //Go through the free spots
            foreach (var key in keys)
            {
                //spot is in use of big item group
                if (blockedPositions.Contains(key))
                {
                    continue;
                }

                //get value of key (direction)
                Direction dir = freeItemSpots[key];

                //set rotation
                Quaternion rotation = GetRotation(dir);

                //Go through every item in itemgroup to see what to spawn
                for (int i = 0; i < itemGroups.Length; i++)
                {
                    //can we spawn as many as possible
                    if (itemGroups[i].amount == -1)
                    {
                        SpawnPrefab(itemGroups[i].GetPrefab(), key, rotation,false);
                        break;
                    }

                    //is there groups left for this itemgroup
                    if (itemGroups[i].hasItems())
                    {
                        //does it require more than one block
                        if (itemGroups[i].horizontalSizeRequired > 1 || itemGroups[i].verticalSizeRequired > 1)
                        {
                            //temporary list of possibly blocked spots
                            List<Vector3Int> tempBlocked = new List<Vector3Int>();

                            //Will the itemgroup fitt the free spot
                            if (VerifyTheGroupFits(itemGroups[i].horizontalSizeRequired, itemGroups[i].verticalSizeRequired, key, dir, ref tempBlocked))
                            {
                                GameObject itemGroup;
                                //are we placing a default item (non buyable)
                                if (itemGroups[i].defaultItem == true)
                                {
                                    itemGroup = SpawnPrefab(itemGroups[i].GetPrefab(), key, rotation, false);
                                }
                                else
                                {
                                    itemGroup = SpawnPrefab(itemGroups[i].GetPrefab(), key, rotation, true);
                                }
                                    
                                //group fits -> add to the blocked list
                                blockedPositions.AddRange(tempBlocked);

                                //add positions to spawned groups
                                foreach (Vector3Int pos in tempBlocked)
                                {
                                    spawnedGroups.Add(pos, itemGroup);
                                }
                                break;
                            }
                        }
                        else
                        {
                            if (itemGroups[i].defaultItem == true) {
                                SpawnPrefab(itemGroups[i].GetPrefab(), key, rotation, false);
                            }
                            else
                            {
                                SpawnPrefab(itemGroups[i].GetPrefab(), key, rotation, true);
                            }
                            break;
                        }
                    }
                }
            }

            //all items spawned -> call item handler
            itemHandler.allSpawned();
        }

        //Spawn the prefab and add to dictionary, ref:1, Sunny Valley Studio, took inspiration for the code
        private GameObject SpawnPrefab(GameObject prefab, Vector3Int position, Quaternion rotation, bool addToSpawned)
        {
            GameObject itemGroup = Instantiate(prefab, position, rotation, transform);
            if (addToSpawned == true)
            {
                spawnedGroups.Add(position, itemGroup);
                itemHandler.AddSpawned(position, itemGroup);
            }
            return itemGroup;
        }

        //Will the Item group fitt, ref:1, Sunny Valley Studio, took inspiration for the code
        private bool VerifyTheGroupFits(
            int horizontal,
            int vertical,
            Vector3Int key,
            Direction dir,
            ref List<Vector3Int> tempBlocked)
        {
            //Count the center of the item (horizontal)
            int horHalfSize = Mathf.FloorToInt(horizontal / 2.0f);

            Vector3Int direction = Vector3Int.zero;

            //determine the direction that item group would be spawned
            if (dir == Direction.Down || dir == Direction.Up)
            {
                direction = Vector3Int.right;
            }
            else
            {
                direction = new Vector3Int(0, 0, 1);
            }

            for (int i = 1; i <= horHalfSize; i++)
            {
                //pos right and left
                var pos1 = key + direction * i;
                var pos2 = key - direction * i;

                //doesn't fit
                if (!freeItemSpots.ContainsKey(pos1) || !freeItemSpots.ContainsKey(pos2) || blockedPositions.Contains(pos1) || blockedPositions.Contains(pos2))
                {
                    return false;
                }

                //add positions to the temporarely blocked
                tempBlocked.Add(pos1);
                tempBlocked.Add(pos2);
            }

            //take store limits
            float zMaxlimit = aisleHelper.zMaxlimit;
            float zMinlimit = aisleHelper.zMinlimit;
            float xMinLimit = aisleHelper.xMinLimit;
            float xMaxLimit = aisleHelper.xMaxLimit;

            //set new direction
            if (direction == Vector3Int.right)
            {
                direction = new Vector3Int(0, 0, 1);
            }
            else
            {
                direction = Vector3Int.right;
            }

            //check if item goes outside the store
            for (int i = 1; i <= vertical; i++)
            {
                //Only go back
                var pos1 = key + direction * i;

                //goes outside store limits?
                if (zMaxlimit <= pos1.z || xMaxLimit <= pos1.x || xMinLimit >= pos1.x || zMinlimit >= pos1.z || zMinlimit >= pos1.z)
                {
                    return false;
                }

                //in blocked or a road
                if (blockedPositions.Contains(pos1) ||  aislePlaces.Contains(pos1))
                {
                    return false;
                }

                //add positions to the temporarely blocked
                tempBlocked.Add(pos1);

            }

            return true;
        }


        //Find the free spots that itemgroups can be spawned in, ref:1, Sunny Valley Studio, took inspiration for the code
        private Dictionary<Vector3Int, Direction> FindFreeSpots(List<Vector3Int> aisles)
        {
            //make the new dictionary for free spaces
            Dictionary<Vector3Int, Direction> freeSpaces = new Dictionary<Vector3Int, Direction>();

            //go through the aisle positions
            foreach (var pos in aisles)
            {
                //get taken positions for the position
                List<Direction> taken = PlacementHelper.FindTaken(pos, aisles);

                //loops through the Direction enum
                foreach (Direction direction in System.Enum.GetValues(typeof(Direction)))
                {
                    //direction not taken -> free space
                    if (!taken.Contains(direction))
                    {
                        Vector3Int newPosition = pos + PlacementHelper.GetOffsetFromDirection(direction);

                        //is there all ready an item here
                        if (freeSpaces.ContainsKey(newPosition))
                        {
                            continue;
                        }

                        //add to the freespaces
                        freeSpaces.Add(newPosition, PlacementHelper.GetReversedDirection(direction));
                    }
                }
            }
            return freeSpaces;
        }

        //get the right rotation for the spawned prefab
        private Quaternion GetRotation(Direction freeSpot)
        {
            switch (freeSpot)
            {
                case Direction.Right:
                    return Quaternion.Euler(0, -90, 0);
                case Direction.Left:
                    return Quaternion.Euler(0, 90, 0);
                case Direction.Up:
                    return Quaternion.Euler(0, 180, 0);
                default:
                    return Quaternion.identity;
            }
        }

        //Randomizing the given list, refs: 2 used 
        private List<Vector3Int> Randomize(List<Vector3Int> list)
        {
            System.Random ran = new System.Random();
            int size = list.Count;
            while (size > 1)
            {
                size--;
                int index = ran.Next(size + 1);
                Vector3Int temp = list[index];
                list[index] = list[size];
                list[size] = temp;
            }
            return list;
        }

        #endregion
    }
}