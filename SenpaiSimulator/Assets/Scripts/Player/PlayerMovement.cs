using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : PlayerState
{

    CharacterController charControl;
    public float walkSpeed;


    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void MovePlayer()
    {
        float horiz = Input.GetAxis("Horizontal");
        float vert = Input.GetAxis("Vertical");
        //Get the horizontal and vertical axis respectively.

        Vector3 moveDirSide = transform.right * horiz * walkSpeed;
        //Represents the direction a player should be going in the horizontal axis.
        Vector3 moveDirForward = transform.forward * vert * walkSpeed;
        //Transform.forward and right represent their respective vectors.

        charControl.SimpleMove(moveDirSide);
        charControl.SimpleMove(moveDirForward);
        //Allows the character controller to move forwards and to the side.
    }
}