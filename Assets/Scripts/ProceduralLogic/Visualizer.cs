using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Visualizer for the L-system. Uses helpers to greate the market.
 * 
 * Miia Remahl 
 * mrema003@gold.ac.uk
 * Last edited: 2.1.2021
 * 
 * References:
 * 1. Sunny Valley Studio - Procedural town : https://www.youtube.com/watch?v=umedtEzrpvU&list=PLcRSafycjWFcbaI8Dzab9sTy5cAQzLHoy&index=1,
 * for creating the procedural base, made some changes to fit the game.
 * The tutorial makes a town but I used it to make the market change every time game starts. Also removed some unnecessary logic.
 */

namespace ProceduralLogic
{
    public class Visualizer : MonoBehaviour
    {
        #region Public variables

        [Header(header: "L-system placement")]
        //Starting position
        public Vector3 startingPosition = new Vector3(2.47f,0f,-13.49f);

        [Header(header: "L-system objects")]
        //L-system reference
        public LsystemGenerator lsystem;

        //Aisle helper
        public AisleHelper aisleHelper;

        //Item group helper
        public ItemGroupHelper itemGroupHelper;

        [Header(header: "L-system variables")]

        //angle to turn
        public float angle = 90;

        //start length of line
        public int startLength = 8;

        #endregion

        #region Private variables

        //length of the aisle
        private int length = 8;

        //saved points
        Stack<AgentParameters> savePoints;

        #endregion

        #region  functions

        //refs: 1.Sunny Valley Studio
        private void Start()
        {
            savePoints = new Stack<AgentParameters>();
            length = startLength;
            VisualizeSequence(lsystem.Generate());
        }

        //Change the length if new values is under 1, refs: 1.Sunny Valley Studio
        private void ChangeLength(int newlength)
        {
            length = newlength > 0 ? newlength : 1;
        }

        //Vizualizes generated sequence, refs: 1.Sunny Valley Studio
        private void VisualizeSequence(string sequence)
        {
            //set current position to starting position
            var currentPosition = startingPosition;

            //starting direction z-axis
            Vector3 direction = Vector3.forward;

            //Temporary position
            Vector3 tempPosition = Vector3.zero;

            //Go through each letter in the generated sequence
            foreach (var letter in sequence)
            {
                EncodingLetters encoding = (EncodingLetters)letter;
                switch (encoding)
                {
                    //save 
                    case EncodingLetters.save:
                        savePoints.Push(new AgentParameters
                        {
                            position = currentPosition,
                            direction = direction,
                            length = this.length
                        });
                        break;
                    //load the saved points
                    case EncodingLetters.load:
                        if (savePoints.Count > 0)
                        {
                            AgentParameters agentParameter = savePoints.Pop();
                            currentPosition = agentParameter.position;
                            direction = agentParameter.direction;
                            ChangeLength(agentParameter.length);
                        }
                        break;
                    //place the aisles
                    case EncodingLetters.draw:
                        aisleHelper.placeAisles(currentPosition, Vector3Int.RoundToInt(direction), length);
                        currentPosition += direction * length;
                        ChangeLength(length - 2);
                        break;
                    //change direction, right
                    case EncodingLetters.turnRight:
                        direction = Quaternion.AngleAxis(angle, Vector3.up) * direction;
                        break;
                    //change direction, left
                    case EncodingLetters.turnLeft:
                        direction = Quaternion.AngleAxis(-angle, Vector3.up) * direction;
                        break;
                    default:
                        break;
                }
            }

            //Place item groups
            itemGroupHelper.PlaceItemGroups(aisleHelper.GetPositions());
        }
        #endregion
    }
}
