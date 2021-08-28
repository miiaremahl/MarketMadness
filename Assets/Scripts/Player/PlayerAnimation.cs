using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * Animation helper for player animation.
 * 
 * Miia Remahl 
 * mrema003@gold.ac.uk
 * Last edited: 24.1.2021
 * 
 */

public class PlayerAnimation : MonoBehaviour
{
    //Player animation
    public Animator animator;

    //Start running
    public void StartRunning()
    {
        animator.SetBool("Running", true);
        animator.SetBool("InNormal", false);
    }

    //From running to normal
    public void BackToNormal()
    {
        animator.SetBool("Running", false);
        animator.SetBool("InNormal", true);
    }

    //pick small item
    public void PickUpSmall()
    {
        animator.SetBool("Running", false);
        animator.SetBool("InNormal", false);
        animator.SetBool("PickingUpSmall", true);
    }

    //pick big item
    public void PickUpBig()
    {
        animator.SetBool("Running", false);
        animator.SetBool("InNormal", false);
        animator.SetBool("PickingUpBig", true);
    }

    //Lose item
    public void LoseItem()
    {
        animator.SetBool("LostItem", true);
        animator.SetBool("PickingUpBig", false);
        animator.SetBool("PickingUpSmall", false);
    }

    //Start fighting
    public void Punch()
    {
        animator.SetBool("InNormal", false);
        animator.SetBool("Running", false);
        animator.SetBool("Punching", true);
    }

    //when ending fighting
    public void EndFight()
    {
        animator.SetBool("Punching", false);
        animator.SetBool("InNormal", true);
    }
}
