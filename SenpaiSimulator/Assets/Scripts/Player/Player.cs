using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance { get; set; } //Singleton declaration

    //inspector-initialized variables
    public float walkSpeed = 8.0f;
    public float gravity = 9.8f;
    public float jumpForce = 20f;
    public float mouseSensitivity = 5;
    public float interactRange = 5;
    public Camera cam;
    public GameObject targetObject;

    //internal variables
    float downMomentum;
    CharacterController charControl;
    GameObject player;
    Vector3 moveDirection = Vector3.zero;
    ManagedUpdate updater;

    //shite state machine setup
    public State state;
    public enum State
    {
        FREE,
        TALKING,
        INVENTORY,
        PAUSE
    }

    private void Awake()
    {
        Instance = this; //Singleton initialization
    }

    //initializes character controller and adds this class to the UpdateManager's list
    private void Start()
    {
        charControl = GetComponent<CharacterController>();
        UpdateManager.AddManagedUpdate(this, updater);
    }

    //managed update method
    public void UpdateThis()
    {
        //rudimentary state machine to control player movement
        switch (state)
        {
            case State.FREE:
                Cursor.lockState = CursorLockMode.Locked; //locks cursor to center of screen
                PlayerMovement();
                PlayerLook();
                PlayerCrouch();
                PlayerCursor();
                break;

            case State.TALKING:
                Cursor.lockState = CursorLockMode.None; //unlocks cursor
                break;

            case State.INVENTORY:
                Cursor.lockState = CursorLockMode.None; //unlocks cursor
                break;


            //defaults the state to FREE if state is somehow undefined
            default:
                state = State.FREE;
                break;
        }
    }

    //moves the player using the CharacterController module and InputManager provided by Unity
    void PlayerMovement()
    {
        moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")); //creates new vector from input
        moveDirection = transform.TransformDirection(moveDirection); //converts local vector to world vector
        moveDirection *= walkSpeed; //sets vector magnitude to walk speed

        if (charControl.isGrounded)
        {
            downMomentum = 0; //resets if player hits the ground
            
            //jump function
            if (Input.GetKeyDown(KeyCode.Space))
            {
                downMomentum += jumpForce;
            }
        }

        moveDirection.y = downMomentum; //carries over from last frame

        moveDirection.y -= gravity * Time.deltaTime; //gravity calculation

        downMomentum = moveDirection.y; //sets to be carried over to next frame

        charControl.Move(moveDirection * Time.deltaTime); //applies movement calculations from above
    }

    //lets player crouch
    //reduces walk speed and collider height by half
    //lowers camera
    //disables jumping
    void PlayerCrouch()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            charControl.height /= 2;
            walkSpeed /= 2;
            jumpForce /= 1000;
        }
        if (Input.GetKeyUp(KeyCode.C))
        {
            charControl.height *= 2;
            walkSpeed *= 2;
            jumpForce *= 1000;
        }
    }

    //allows the player to look around with the mouse, while also rotating the playermodel accordingly
    void PlayerLook()
    {

        //gets the delta movement of the mouse as usable variables
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");
        
        //multiplies mouse movement by mouse sensitivity
        float rotX = mouseX * mouseSensitivity;
        float rotY = mouseY * mouseSensitivity * -1; //inverted due to the way rotation works in Unity

        //gets current rotation of the camera and player objects
        //converts output from Quaternions to degrees
        Vector3 targetRotCam = cam.transform.rotation.eulerAngles;
        Vector3 targetRotBody = transform.rotation.eulerAngles;

        //adds the change provided by the input to the current rotation
        //the playermodel rotates horizontally with the input
        //the camera's horizonal rotation (y) is always 0, since it is attached to the playermodel
        targetRotCam.x += rotY; 
        targetRotBody.y += rotX;
        targetRotCam.z = 0; //this keeps the camera rightside-up

        //clamps the camera so the player can never look beyond straight up and straight down
        if (targetRotCam.x > 90 && targetRotCam.x < 180)
        {
            targetRotCam.x = 90;
        }
        if (targetRotCam.x < 270 && targetRotCam.x > 180)
        {
            targetRotCam.x = 270;
        }

        //applies changes to the rotation of the camera and player objects
        //converts degrees back to Quaternions
        cam.transform.rotation = Quaternion.Euler(targetRotCam);
        transform.rotation = Quaternion.Euler(targetRotBody);
    }

    void PlayerCursor()
    {
        //casts a ray from the center of the camera
        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactRange))
        {
            print("I'm looking at " + hit.transform.name); //for debugging purposes, spits out the current object the player is looking at

            targetObject = GameObject.Find(hit.transform.name); //sets public variable to the GameObject being looked at
        }
        else
        {
            print("I'm looking at nothing!");
            targetObject = null;
        }
    }

}