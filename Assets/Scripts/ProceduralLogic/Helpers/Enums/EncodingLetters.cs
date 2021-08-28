using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * EncodingLetters enum. Used for L-system.
 * 
 * Miia Remahl 
 * mrema003@gold.ac.uk
 * Last edited: 30.12.2020
 * 
 * References:
 * 1. Sunny Valley Studio - Procedural town : https://www.youtube.com/watch?v=umedtEzrpvU&list=PLcRSafycjWFcbaI8Dzab9sTy5cAQzLHoy&index=1,
 * for the encoding example.
 */

namespace ProceduralLogic
{
    public enum EncodingLetters
    {
        save = '[',
        load = ']',
        draw = 'F',
        turnRight = '+',
        turnLeft = '-',
    }
}
