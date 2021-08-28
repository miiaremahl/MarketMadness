using UnityEngine;

/*
 * Class for the interactions.
 * Miia Remahl 
 * mrema003@gold.ac.uk
 * last edited: 9.1.2021
 * 
 * Refs:
 * 1.Brackeys - INTERACTION - Making an RPG in Unity (E02) :https://www.youtube.com/watch?v=9tePzyL6dgc&list=PLPV2KyIb3jR4KLGCCAciWQ5qHudKtYeP7&index=3, used for making the interactable class
 * 2.Brackeys - ITEMS - Making an RPG in Unity (E04) ,https://www.youtube.com/watch?v=HQNl3Ff2Lpo&list=PLPV2KyIb3jR4KLGCCAciWQ5qHudKtYeP7&index=5&t=21s, used for making the class fit items
 * 
 */

public class Interactable : MonoBehaviour
{
    [Header(header: "Interaction")]
    //How close we need to be to interact
    public float interactionRad = 3f;

    [Header(header: "Object refs")]
    //ref to player
    public Transform player;

    //Which place should the object be interacted from
    public Transform interactionPos;

    //is being focused
    private bool isFocus = false;

    //has the object been inteacting w the focusing object
    private bool interacted = false;

    #region Override
    //Meant to override
    public virtual void Interact()
    {
        interacted = true;
    }
    #endregion

    void Update()
    {
        if (isFocus && !interacted)
        {
            float dist = Vector3.Distance(interactionPos.position, transform.position);
            if (dist <= interactionRad) //if we are close enough interact
            {
                Interact();
            }
        }
    }

    //Focus started, refs: 1.Brackeys 
    public void Focused(Transform playerTransform)
    {
        isFocus = true;
        player = playerTransform;
        interacted = false;
    }

    //Focus ended, refs: 1.Brackeys 
    public void DeFocused()
    {
        isFocus = false;
        player = null;
        interacted = false;
    }

}
