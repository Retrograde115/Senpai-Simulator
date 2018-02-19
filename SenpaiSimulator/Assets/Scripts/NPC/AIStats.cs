using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//In the tutorial it's referred to as EnemyStats
namespace SA.AI
{
    public class AIStats : MonoBehaviour
    {
        public List<WaypointsBase> waypoints = new List<WaypointsBase>();
        [HideInInspector]
        public WaypointsBase curWaypoint;
        public int waypointIndex;
        public float waypointDistance;
    }
}