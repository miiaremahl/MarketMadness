using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * 
 * Miia Remahl 
 * mrema003@gold.ac.uk
 * Last edited: 2.1.2020
 * 
 * References:
 * 1. Sunny Valley Studio - Procedural town : https://www.youtube.com/watch?v=umedtEzrpvU&list=PLcRSafycjWFcbaI8Dzab9sTy5cAQzLHoy&index=1 ,
 * used to make the basis for the procedural generation (tutorial for town -> modified for market)
 */

namespace ProceduralLogic
{
    public class AgentParameters : MonoBehaviour
    {
        //Position
        public Vector3 position;

        //Direction to go
        public Vector3 direction;

        //Length
        public int length;
    }
}
