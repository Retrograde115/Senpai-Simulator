using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    CharacterController charControl;
    //Private because we aren't touching this in the inspector.
    public float walkSpeed;
    //Keeps track of our walkspeed.
    //Don't forget to set the walkspeed to above zero in the inspector.

	void Awake()
    {
        charControl = GetComponent<CharacterController>();
    }

    void Update()
    {
        MovePlayer();
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
