using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/*
 * TypeWriter class. For generating typewrited text.
 * Miia Remahl 
 * mrema003@gold.ac.uk
 * last edited: 4.1.2021
 * 
 * References:
 * 1. Code Monkey - How to make Text Writing Effect in Unity, https://www.youtube.com/watch?v=ZVh4nH8Mayg : took ideas dor the type writing effect and tried to make it work for my game.
 */


public class TypeWriter : MonoBehaviour
{
    #region private variables

    //list of typewriters
    private List<TypeWriterSingle> typeWriterList;

    //instance of a typewriter
    private static TypeWriter instance;

    #endregion

    #region Awake / Update
    //setting up
    private void Awake()
    {
        instance = this;
        typeWriterList = new List<TypeWriterSingle>();
    }


    private void Update()
    {
        //remove all the writers that are not needed
        for (int i = 0; i < typeWriterList.Count; i++)
        {
            if (typeWriterList[i].Update())
            {
                typeWriterList.RemoveAt(i);
                i--;
            }
        }
    }
    #endregion

    #region public/private Functions

    //static method for removing the writer 
    public static void RemoveWriter_Static(TextMeshProUGUI uiText)
    {
        instance.RemoveWriter(uiText);
    }

    //static method for adding a writer
    public static TypeWriterSingle AddWriter_Static(TextMeshProUGUI uiText, 
        string textToWrite, float timePerCharacter,
        bool removeWriterBeforeAdd,
        Action onComplete)
    {
        //is there an old writer
        if (removeWriterBeforeAdd)
        {
            instance.RemoveWriter(uiText);
        }
        return instance.AddWriter(uiText, textToWrite, timePerCharacter, onComplete);
    }

    //removing writer with given text
    private void RemoveWriter(TextMeshProUGUI uiText)
    {
        for (int i = 0; i < typeWriterList.Count; i++)
        {
           if(typeWriterList[i].GetTextObj() == uiText)
            {
                typeWriterList.RemoveAt(i);
                i--;
            }
        }
    }

    //adds a text writer
    private TypeWriterSingle AddWriter(TextMeshProUGUI uiText, string textToWrite, float timePerCharacter, Action onComplete)
    {
        TypeWriterSingle writer = new TypeWriterSingle(uiText, textToWrite, timePerCharacter, onComplete);
        typeWriterList.Add(writer);
        return writer;
    }

    #endregion

    #region TextWriterSingle class

    //Nested class of one typewriter instance
    public class TypeWriterSingle
    {
        #region private variables
        //where text is displayed
        private TextMeshProUGUI instruction;

        //what text should be written
        private string textToWrite;

        //index of the written character
        private int charIndex;

        //time spend to write each character
        private float timePerCharacter;

        //timer
        private float timer;

        //action when writing is completed
        private Action onComplete;
        #endregion

        #region Functions

        //create typewriter
        public TypeWriterSingle(TextMeshProUGUI uiText, string textToWrite, float timePerCharacter, Action onComplete)
        {
            instruction = uiText;
            this.textToWrite = textToWrite;
            this.timePerCharacter = timePerCharacter;
            charIndex = 0;
            this.onComplete = onComplete;
        }

        //Return true when writing is completed
        public bool Update()
        {
            timer -= Time.deltaTime;

            //is the waiting time passed
            if (timer <= 0f)
            {
                //writing is done
                if (charIndex >= textToWrite.Length)
                {
                    if (onComplete != null) onComplete();
                    return true;
                }

                //new time and character to write
                timer += timePerCharacter;
                charIndex++;

                //make the new displayed message (visible text) + (invisible text)
                instruction.text = $"{textToWrite.Substring(0, charIndex)}<color=#00000000>{textToWrite.Substring(charIndex)}</color>";
            }
            return false;
        }

        //get the text object
        public TextMeshProUGUI GetTextObj()
        {
            return instruction;
        }

        //Is writer still writing
        public bool IsActive()
        {
            return charIndex < textToWrite.Length;
        }

        //write all the text and destroy object
        public void WriteAll()
        {
            instruction.text = textToWrite;
            charIndex = textToWrite.Length;
            if (onComplete != null) onComplete();
            TypeWriter.RemoveWriter_Static(instruction);
       }
        #endregion
    }
    #endregion

}
