//If you watch the tutorial he uses EnemyAI. But I've named the script AI.
//Temporarily disabled.

/*
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIDirector : MonoBehaviour

{
    public Transform playerTarget;
    public List<AI> all = new List<AI>();
    List<AI> close = new List<AI>();
    List<AI> far = new List<AI>();

    //This removes some redundency from the AI script.
    List<AI> candidatesFromCloseToFar = new List<AI>();
    List<AI> candidatesFromFarToClose = new List<AI>();

    //The cycles make the AI run X number of frames.
    int closeCycle = 10;
    int cl; //Short for close because we used close above.
    int farCycle = 60; //At 60FPS, we are losing ~1FPS to check for far away AI.
    int fr; //Short for far because we used far above.

    //The minimal distance to determine if the AI is within the close or far distance.
    float maxDistance = 25;

    void Start()
    {
        playerTarget = GameObject.FindGameObjectWithTag("Player").transform; //No need to assign to the script. Searches for object designated as player.

        //Apparently we're using FindObjectsofType to show why it's so shit. We'll be replacing it later.
        AI[] a = GameObject.FindObjectsOfType<AI>();
        all.AddRange(a);

        //This determines if an AI is placed on the close or far list.
        for (int i = 0; i < all.Count; i++)
        {
            float distance = Vector3.Distance(playerTarget.position, all[i].transform.position);

            if (distance < maxDistance)
                close.Add(all[i]);
            else
                far.Add(all[i]);

            all[i].playerTarget = playerTarget.transform; //Need this otherwise the AI won't know to target the player.
        }
    }

    void Update()
    {
        //Each frame we'll be adding to both the close and far integers.
        cl++;
        fr++;

        //If the close integer is greater than the cycle, reset.
        if(cl > closeCycle)
        {
            cl = 0;
            for (int i = 0; i < close.Count; i++) //This could also be turned into a while loop in theory.
            {
                close[i].Tick();

                float distance = Vector3.Distance(playerTarget.position, close[i].transform.position);

                if (distance > maxDistance)
                    candidatesFromCloseToFar.Add(close[i]);
            }

            for(int i = 0; i < candidatesFromCloseToFar.Count; i++)
            {
                close.Remove(candidatesFromCloseToFar[i]);
                far.Add(candidatesFromCloseToFar[i]);
            }

            return; //Optimization. Disables the overlapping from below.
        }

        if (fr > farCycle)
        {
            fr = 0;
            for (int i = 0; i < far.Count; i++)
            {
                far[i].Tick();

                float distance = Vector3.Distance(playerTarget.position, close[i].transform.position);

                if (distance < maxDistance)
                    candidatesFromFarToClose.Add(far[i]);
            }

                for (int i = 0; i < candidatesFromFarToClose.Count; i++)
                {
                    close.Remove(candidatesFromFarToClose[i]);
                    far.Add(candidatesFromFarToClose[i]);
                }
            }
        }
}
*/