using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/*
 * OLD SCRIPT FOR MOVEMENT.
 * Miia Remahl 
 * mrema003@gold.ac.uk
 * last edited: 24.1.2020 -> removed from the use
 * 
 * References:
 * 1.TKGgames- Simple Rigidbody Character Controller for Unity3D ,https://www.youtube.com/watch?v=NEUzB5vPYrE : for making rigidbody controller basics and made some chances for it.
 * 2. Brackeys - FIRST PERSON MOVEMENT in Unity - FPS Controller : Used this for ground checking.
 * 3. incorrect - Rotating rigidbody to follow Mouse X ,https://answers.unity.com/questions/829688/rotating-rigidbody-transform-to-match-rotation-of.html : used for the MouseX rotation
 */

namespace Player
{
    public class RigidBodyMovement : MonoBehaviour
    {
        #region OLD
        //movement in x ja z axis
        private float xAxis;
        private float zAxis;

        //what the player hit
        private RaycastHit _hit;
        private Vector3 _groundLocation;
        public LayerMask groundMask; //ground mask
        public bool isGrounded; //Is player touching ground


        //is shift key down
        private bool shiftDown;

        //game paused
        private bool gamePaused = false;

        public Transform playerCam;
        public Transform orientation;
        public Rigidbody rb;

        //speed of player
        [Header(header: "Speed")]
        public float walkSpeed;
        public float runSpeed;
        public float currentSpeed;
        private float maxSpeed = 20;

        //jumping
        [Header(header: "Jumping")]
        public float JumpForce;
        public ForceMode Force;
        public bool Jumping;

        //groundcheckers position
        public Transform groundCheck;

        //how big distance is being checked
        public float checkDistance = 0.4f;


        void Start()
        {
            //lock the cursor
            Cursor.lockState = CursorLockMode.Locked;
        }

        //refs: 1.TKGgames
        private void Update()
        {
            if (!gamePaused)
            {
                //get movement input x and z axis
                xAxis = Input.GetAxisRaw("Horizontal");
                zAxis = Input.GetAxisRaw("Vertical");

                //check if jumping button is pressed
                Jumping = Input.GetButton("Jump");

                //check if shift is being pressed
                shiftDown = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift); 
                currentSpeed = shiftDown ? runSpeed : walkSpeed;  //adjust the speed
            }
        }

        //refs: 1.TKGgames, 2. Brackeys
        private void FixedUpdate()
        {
            if (!gamePaused)
            {
                //check collision with ground mask
                isGrounded = Physics.CheckSphere(groundCheck.position, checkDistance, groundMask);

                #region OLD Code
                //Movement applied to rigidbody
                rb.MovePosition(transform.position + Time.deltaTime * currentSpeed * transform.TransformDirection(xAxis, y: 0f, zAxis));
                #endregion


                //jumping: is touching ground and jump pressed
                if (Jumping && isGrounded)
                {
                    Jump(JumpForce, Force);
                }

                #region OLD CODE


                rb.rotation = Quaternion.Euler(rb.rotation.eulerAngles + new Vector3(0f, 10 * Input.GetAxis("Mouse X"), 0f));
                #endregion
            }
        }

        //pause game
        public void Pause()
        {
            gamePaused = true;
        }

        //continue game
        public void Continue()
        {
            gamePaused = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        //player jumping -> add force to rigidbody
        //refs: 1.TKGgames
        private void Jump(float jumpForce, ForceMode forceMode)
        {
            rb.AddForce(jumpForce * rb.mass * Time.deltaTime * Vector3.up, forceMode);
        }
        #endregion
    }
}
