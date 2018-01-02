using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMotor : PlayerState
{

    private CharacterController controller;

    private float verticalVelocity;
    public float gravity = 28.0f;
    //14 is a bit floaty. I have doubled it from the tutorial amount. I'm going to allow us to adjust in the inspector.
    public float jumpForce = 50.0f;
    //How hard you jump off the ground.


    private void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        
        if(controller.isGrounded && state == State.FREE)
            //Is the player on the ground?
        {
            verticalVelocity = -gravity * Time.deltaTime;
            //If he is, there is also pressure on him to ensure he stays there.
            if(Input.GetKeyDown(KeyCode.Space))
            {
                verticalVelocity = jumpForce * Time.deltaTime;
                //If you press space, propel the object into the air.
            }
            /*else
            {
                verticalVelocity -= gravity * Time.deltaTime;
                //But the longer he's in the air, the faster he accelerates towards the ground.
            }*/

            Vector3 moveVector = new Vector3(0, verticalVelocity, 0);
            controller.Move(moveVector * Time.deltaTime);
            //I actually don't know what this chunk is for.

        }
    }
}