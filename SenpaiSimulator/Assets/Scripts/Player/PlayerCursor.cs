using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//script used to interact with objects
//must be attached to the same object the player camera is on
public class PlayerCursor : MonoBehaviour {

    public static GameObject targetObject;
    Camera cam;
    public int interactRange;
    
	void Start ()
    { 
        //initializes cam variable to the Camera object on current GameObject
        cam = GetComponent<Camera>(); 
	}
	
	void Update ()
    {
        //casts a ray from the center of the camera
        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;
        
        if (Physics.Raycast(ray, out hit, interactRange))
        {
            //for debugging purposes, spits out the current object the player is looking at
            print("I'm looking at " + hit.transform.name);
            targetObject = GameObject.Find(hit.transform.name);
        }
        else
        {
            print("I'm looking at nothing!");
            targetObject = null;
        }
	}
}
