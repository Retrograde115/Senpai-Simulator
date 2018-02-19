using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA.AI
{
    public static class CommonBehaviours
    {
        //This could be shoved into it's own script.
        //Might be more compact in the future
        public static void PatrolBehaviour(AI ai)
        {
            AIStats e = ai.aiStats;

            if (e.waypoints.Count > 0)
            {
                if (e.curWaypoint.targetDestination == null)
                {
                    e.curWaypoint = e.waypoints[0];
                    ai.MoveToPosition(e.curWaypoint.targetDestination.position);
                }
                e.curWaypoint = e.waypoints[e.waypointIndex];
                e.waypointDistance = Vector3.Distance(ai.transform.position, e.curWaypoint.targetDestination.position);

                Debug.Log(e.waypointDistance);

                if(e.waypointDistance < 2)
                {
                    if(e.waypointIndex < e.waypoints.Count -1)
                    {
                        e.waypointIndex++;       
                    }
                    else
                    {
                        e.waypointIndex = 0;
                        e.waypoints.Reverse(); //Turnaround. But simply removing this causing a circle.
                    }

                    e.curWaypoint = e.waypoints[e.waypointIndex];
                    ai.MoveToPosition(e.curWaypoint.targetDestination.position);
                }
            }
        }

    }

    [System.Serializable]
    public class WaypointsBase
    {
        public Transform targetDestination;
    }
}