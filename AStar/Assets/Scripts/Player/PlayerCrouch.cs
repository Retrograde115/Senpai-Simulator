using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCrouch : MonoBehaviour

{

    CharacterController characterCollider;

    void Start ()
    {
        characterCollider = gameObject.GetComponent<CharacterController>();
        //Allows us to adjust in the inspector?
    }

    void Update ()
    {
        if (Input.GetKey(KeyCode.C))
            //If you press the C key.
        {
            characterCollider.height = 1.0f;
            //Height becomes this.
        }
        else
        {
            characterCollider.height = 1.8f;
            //If you stop holding C, return to this height.
        }
    }
}
