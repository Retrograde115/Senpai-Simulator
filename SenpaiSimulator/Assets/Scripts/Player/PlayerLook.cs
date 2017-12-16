using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    public Transform playerBody;
    //Without this, the player body is not rotating with the camera. Has an option within the Inspector.
    public float mouseSensitivity;
    //Made public so it can be tinkered with in the Inspector.
    //Currently set to 2. Cannot be zero otherwise the camera won't move because 0 multiplied by anything is zero.
    //Note that the cursor is currently invisible. Though if you want I can find ways to add a custom cursor.

    float xAxisClamp = 0.0f;
    //This stops the seizures when you look too hard up or down.

    void Awake()
        //This is going to lock the mouse within the screen.    
    {
        Cursor.lockState = CursorLockMode.Locked;
        //Putting it here as opposed to Update causes some weird bullshit to happen when alt-tabbing between windows. I think having it in Update will cause the mouse to always be in the centre of the screen.
    }

    void Update()
    {
        RotateCamera();
            //RotateCamera is updated every frame.
    }

    void RotateCamera()
        //This is working with degrees, not radians.
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");
        //Gets movement from the mouse X and Y axis.

        float rotAmountX = mouseX * mouseSensitivity;
        float rotAmountY = mouseY * mouseSensitivity;
        //Determines how much the camera rotates. mouseSensitivity is a multiplier.

        xAxisClamp -= rotAmountY;
        //This stops the seizures when you look too hard up or down.

        Vector3 targetRotCam = transform.rotation.eulerAngles;
        //targetRotCam determines the current rotation of the target object camera.
        Vector3 targetRotBody = playerBody.rotation.eulerAngles;
        //targetRotCam determines the current rotation of the target object body.
        targetRotCam.x -= rotAmountY;
        targetRotBody.y += rotAmountX;
        //This determines the camera rotation based on the original rotation of the target object.
        //Yes, the rotAmounts do not have the corresponding X and Y with the targetRots. This is intentional. If you want the explanation go to about 8:50 in the tutorial video. Has to do with Axis rotation.
        //targetRot.x has a -= because += actually inverts shit.
        //We have the y-chunk rotating the player body.
        targetRotCam.z = 0;
        //The 0 stops the camera from inverting when you look too hard up or down.

        if (xAxisClamp > 90)
        {
            xAxisClamp = 90;
            targetRotCam.x = 90;
        }
        else if (xAxisClamp < -90)
        {
            xAxisClamp = -90;
            targetRotCam.x = 270;
        }
            //If the x axis is looking down too far up or down, just clamp it at exactly up or down.

        transform.rotation = Quaternion.Euler(targetRotCam);
        //Adjust the camera rotation based on above adjustments.
        //Note that we're using Eulers to make the rotation because transform.rotation uses Quaternions. We use the Euler to convert this back into Vector3.
        //Note that if we're transforming the Vector3 back, we need to use Quaternion.Euler to covert it back.
        playerBody.rotation = Quaternion.Euler(targetRotBody);
        //Adjusts the player body based on above adjustments.

    }

}
