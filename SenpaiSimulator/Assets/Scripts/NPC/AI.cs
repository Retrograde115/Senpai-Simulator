using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI : MonoBehaviour

{

    public Transform target;

    void Update()
    {
        if(target)
        {
            Vector3 direction = target.position - transform.position;
            direction.y = 0;
            if (direction == Vector3.zero)
                direction = transform.forward;
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5);
                //This is just to show that the AI can see the target.
                //Difference between having a target and not having one.
        }
    }

}
