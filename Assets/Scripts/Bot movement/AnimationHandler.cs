using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * Animation helper for customer animation.
 * 
 * Miia Remahl 
 * mrema003@gold.ac.uk
 * Last edited: 24.1.2020
 * 
 */

public class AnimationHandler : MonoBehaviour
{
    //Customer animation
    public Animator animator;

    //carry item from shelf
    public void CarryShelfItem()
    {
        animator.SetBool("IsCarrying", true);
        animator.SetBool("IsRunning", false);
    }

    //start running to item
    public void StartRunningToItem()
    {
        animator.SetBool("IsRunning", true);
    }

    //move to player
    public void MoveToPlayer()
    {
        animator.SetBool("IsRunning", true);
    }

    //start running after winning
    public void StartRunningAfterWon()
    {
        animator.SetBool("IsPunching", false);
        animator.SetBool("IsCarrying", true);
    }

    //start running after staying still
    public void StartRunningFromId()
    {
        animator.SetBool("IsIdle", false);
        animator.SetBool("IsRunning", true);
    }

    //start a fight
    public void StartFighting()
    {
        animator.SetBool("IsRunning", false);
        animator.SetBool("IsPunching", true);
    }

    //fall 
    public void Fall()
    {
        animator.SetBool("IsCarrying", false);
        animator.SetBool("IsPunching", false);
        animator.SetBool("IsFalling", true);
    }

    //stand still after getting up
    public void StandStillAfterGU()
    {
        animator.SetBool("IsGettingUp", false);
        animator.SetBool("IsIdle", true);
    }

    //get up
    public void GetUp()
    {
        animator.SetBool("IsFalling", false);
        animator.SetBool("IsGettingUp", true);
    }

    //with when you have item
    public void FightWithItem()
    {
        animator.SetBool("IsCarrying", false);
        animator.SetBool("IsHittingWithItem", true);
    }

    //win fight w item
    public void WinFightWithItem()
    {
        animator.SetBool("IsCarrying", true);
        animator.SetBool("IsHittingWithItem", false);
    }


}
