using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

/*
 * Placement helper class
 * 
 * Miia Remahl 
 * mrema003@gold.ac.uk
 * Last edited: 2.1.2020
 * 
 * References:
 * 1. Sunny Valley Studio - Procedural town : https://www.youtube.com/watch?v=umedtEzrpvU&list=PLcRSafycjWFcbaI8Dzab9sTy5cAQzLHoy&index=1,
 * Used to make tha basis for placement helping, made some modifications.
 */

namespace ProceduralLogic
{
    public static class PlacementHelper
    {

        //Lists taken positions
        //ref:1 Sunny Valley Studio, took inspiration for the code
        public static List<Direction> FindTaken(Vector3Int position, ICollection<Vector3Int> collection)
        {
            //create list for taken
            List<Direction> taken = new List<Direction>();

            //is right direction taken
            if (collection.Contains(position + Vector3Int.right))
            {
                taken.Add(Direction.Right);
            }

            //is left direction taken
            if (collection.Contains(position - Vector3Int.right))
            {
                taken.Add(Direction.Left);
            }

            //is up direction taken
            if (collection.Contains(position + new Vector3Int(0, 0, 1)))
            {
                taken.Add(Direction.Up);
            }

            //is down direction taken
            if (collection.Contains(position - new Vector3Int(0, 0, 1)))
            {
                taken.Add(Direction.Down);
            }
            return taken;
        }

        //returns the Reversed direction
        //ref:1
        internal static Direction GetReversedDirection(Direction direction)
        {
            switch (direction)
            {
                case Direction.Up:
                    return Direction.Down;
                case Direction.Down:
                    return Direction.Up;
                case Direction.Left:
                    return Direction.Right;
                case Direction.Right:
                    return Direction.Left;
                default:
                    break;
            }
            return direction;
        }

        //gets the offset of given direction
        //ref:1 Sunny Valley Studio, took inspiration for the code
        internal static Vector3Int GetOffsetFromDirection(Direction direction)
        {
            //return correct direction vector3Int
            switch (direction)
            {
                case Direction.Up:
                    return new Vector3Int(0, 0, 1);
                case Direction.Down:
                    return new Vector3Int(0, 0, -1);
                case Direction.Left:
                    return Vector3Int.left;
                case Direction.Right:
                    return Vector3Int.right;
                default:
                    break;
            }
            //direction doesnt match item in enum
            throw new Exception();
        }

    }

}
