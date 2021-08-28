using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/*
 * Aisle helper class.
 * 
 * Miia Remahl 
 * mrema003@gold.ac.uk
 * Last edited: 2.1.2020
 * 
 * References:
 * 1. Sunny Valley Studio - Procedural town : https://www.youtube.com/watch?v=umedtEzrpvU&list=PLcRSafycjWFcbaI8Dzab9sTy5cAQzLHoy&index=1,
 * Modified the "roadlogic" from the tutorial by making changes so that the code fits the game better.
 */

namespace ProceduralLogic
{
    public class AisleHelper : MonoBehaviour
    {
        //public GameObject roadPrefab; -> for dev purposes

        #region Variables

        [Header(header: "Store cordinates")]
        //store cordinates (legal areas to place objects)
        public float zMaxlimit;
        public float zMinlimit;
        public float xMinLimit;
        public float xMaxLimit;

        [Header(header: "Fodbidden area cordinates")]
        //Forbidden area for the objects to spawn inside store
        public Vector3[] forbiddenArea = new Vector3[3];

        //adjustment for counting the forbidden area
        private float adjustmentNum = 0.5f;

        //list for aisles
        private List<Vector3Int> aisleCordinates = new List<Vector3Int>();

        #endregion


        #region Functions

        //get the aisle positions
        public List<Vector3Int> GetPositions()
        {
            return aisleCordinates;
        }

        //place the aisles, refs: 1. Sunny Valley Studio - for procedural logic example
        public void placeAisles(Vector3 startPosition, Vector3Int direction, int length)
        {
            //rotation for the prefab
            var rotation = direction.x == 0 ? Quaternion.Euler(0, 90, 0) : Quaternion.identity;

            //place aisles
            for (int i = 0; i < length; i++)
            {
                //calculate position
                var position = Vector3Int.RoundToInt(startPosition + direction * i);

                //check if there already is aisle placed or if the position is illegal
                if (aisleCordinates.Contains(position) || CheckIsInForbiddenArea(position))
                {
                    continue;
                }

                //var road = Instantiate(roadPrefab, position, rotation, transform);

                //add to list
                aisleCordinates.Add(position);
            }
        }

        //checks if the position is in the forbidden area 
        private bool CheckIsInForbiddenArea(Vector3 positionToCheck)
        {
            //out of the store
            if (zMaxlimit <= positionToCheck.z || xMaxLimit <= positionToCheck.x || xMinLimit >= positionToCheck.x || zMinlimit >= positionToCheck.z)
            {
                return true;
            }

            //forbidden area (cashier place)
            if (forbiddenArea[0].z >= positionToCheck.z - adjustmentNum && forbiddenArea[1].z <= positionToCheck.z + adjustmentNum)
            {
                if ((forbiddenArea[1].x) >= positionToCheck.x - adjustmentNum && (forbiddenArea[2].x) <= positionToCheck.x + adjustmentNum)
                {
                    return true;
                }
            }
            return false;
        }

        #endregion
    }
}
