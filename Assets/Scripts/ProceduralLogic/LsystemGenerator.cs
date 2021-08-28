using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

/*
 * Class that generates the L-system. 
 * 
 * Miia Remahl 
 * mrema003@gold.ac.uk
 * Last edited: 2.1.2020
 * 
 * References:
 * 1. Sunny Valley Studio - Procedural town : https://www.youtube.com/watch?v=umedtEzrpvU&list=PLcRSafycjWFcbaI8Dzab9sTy5cAQzLHoy&index=1,
 * Used for making basic L-system logic.
 */

namespace ProceduralLogic
{
    public class LsystemGenerator : MonoBehaviour
    {
        //All rules used in L-system
        public Rule[] rules;

        //Axiom for the l-system (the starting character)
        public string axiom;

        //Iteration limit for the L-system
        public int iterationLimit = 1;

        //change to ignoring a rule
        public float ignoringChance = 0.4f;


        //Start the l-system generation, refs:1 Sunny Valley Studio, took inspiration for the code
        public string Generate()
        {
            return GrowSentence(axiom,0);
        }

        //grown the sentence recursively, refs:1 Sunny Valley Studio, took inspiration for the code
        private string GrowSentence(string word, int iterationIndex)
        {
            //check if we have done enough iterations
            if (iterationIndex >= iterationLimit)
            {
                return word;
            }

            //create new stringbuilder
            StringBuilder stringBuilder = new StringBuilder();

            //go through characters in the word
            foreach (var c in word)
            {
                //add character to the new word
                stringBuilder.Append(c);

                //grow by the rules
                ProcessCharacter(stringBuilder, c, iterationIndex);
            }

            //return the new processed sentence 
            return stringBuilder.ToString();
        }


        //Processess the given character, refs:1 Sunny Valley Studio, took inspiration for the code
        private void ProcessCharacter(StringBuilder stringBuilder, char c, int iterationIndex)
        {
            //loop through the rules
            foreach (var rule in rules)
            {
                //compare the rules letter to character
                if (rule.letter == c.ToString())
                {
                    //Random ignoring of the rule
                    if (iterationIndex > 1)
                    {
                        if (Random.value < ignoringChance)
                        {
                            return;
                        }
                    }

                    //new iteration
                    stringBuilder.Append(GrowSentence(rule.GetResult(), iterationIndex + 1));
                }
            }
        }
    }
}
