using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI : MonoBehaviour

{

    public Transform target;

    bool isInView; //This tells us if the player character is within range.

    //This is a check for optimization
    int cycle = 10;
    int counter = 0;

    void Start()
    {
        //This is breaking up the update cycles for multiple AI, ensuring that not everyone is updating at the same time.
        counter = Random.Range(0, 15);
    }

    void Update()
    {
        //This is optimizing the code, preventing the cycles from going beyond a set number. In this case, 11 times per frame causes a reset.
        counter++; 
        if (counter > cycle)
        {
            counter = 0;
            FieldofView();
        }

        if (isInView)
        {
            Vector3 direction = target.position - transform.position;
            direction.y = 0;
            if (direction == Vector3.zero)
                direction = transform.forward;

            float angle = Vector3.Angle(transform.forward, direction); //This deals with the cone of vision.

            if (angle < 45) //Cone of vision currently set to a 45 degree angle.
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5);
                //This is just to show that the AI can see the target.
                //Difference between having a target and not having one.
            }
        }
    }

    void FieldofView()
    {
        if (target)
        {
            //Establishing the radius of the AI's range
            float distance = Vector3.Distance(target.position, transform.position);
            isInView = (distance < 4);

        }
    }
}