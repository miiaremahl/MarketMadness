using UnityEngine;

/*
 * ScriptableObject rule for the Procedural generation of the market items.
 * 
 * Miia Remahl 
 * mrema003@gold.ac.uk
 * Last edited: 1.1.2021
 * 
 * References:
 * 1. Sunny Valley Studio - Procedural town : https://www.youtube.com/watch?v=umedtEzrpvU&list=PLcRSafycjWFcbaI8Dzab9sTy5cAQzLHoy&index=1,
 * for creating the procedural base, made some changes to fit the game. 
 */

// uses the ref.1 to make the basic rules
namespace ProceduralLogic
{
    [CreateAssetMenu(menuName = "ProceduralMarket/Rule")]

    public class Rule : ScriptableObject
    {
        //The letter that rule applies to
        public string letter;

        [SerializeField]
        //Results for the letter
        private string[] results = null;

        //Gets a random result that the letter should be changed to
        public string GetResult()
        {
            int index = UnityEngine.Random.Range(0, results.Length);
            return results[index];
        }
    }
}

