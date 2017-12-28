////I just typed this up quickly to transfer over to inventory. This isn't actually used anywhere.

//using System.Collections;
//using System.Collections.Generic;
////Have to define the UI before we can use it.
//using UnityEngine.UI;

//public class UIFollowMouse : MonoBehaviour

//{

//    //The moving chunk guided by the mouse.
//    public RectTransform MovingObject;
//    //this shifts geometry, though I'm not sure for what or why.
//    public Vector3 offset;
//    //This is like the parent object. Ensures the Z axis of the dragged object stays put.
//    public RectTransform BasisObject;
//    //Use this to translate the screen coordinates to an actual point.
//    public Camera cam;

	
//	// Update is called once per frame
//	void Update ()
//    {
//        //Just moves the object.
//        MoveObject();
//	}

//    public void MoveObject()
//    {
//        //Establish the position of the object. Offset allows us to change the mouse position while in-game.
//        Vector3 pos = Input.mousePosition + offset;
//        //Reset the z variable or depth which isn't being used.
//        pos.z = BasisObject.position.z;
//        //Set the position towards the camera.
//        MovingObject.position = cam.ScreenToWorldPoint(pos);
//    }
//}