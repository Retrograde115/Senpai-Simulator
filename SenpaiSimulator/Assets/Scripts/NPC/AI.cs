using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace SA.AI
{
    public class AI : MonoBehaviour
    {
        //If there are multiple targets we'll likely need to turn this chunk into its own class, make a list/array, so we can check for all viable targets.
        public Transform playerTarget;
        Vector3 direction;
        Vector3 rotDirection;
        float distance;
        Vector3 lastKnownPosition;

        bool isInView;
        bool isInAngle;
        bool isClear;
        float radius = 30;
        float maxDistance = 50;

        //This is a check for optimization. Adds X amount per frame.
        int lFrame = 15;
        int lFrame_counter = 0;

        int llateFrame = 35;
        int llate_counter = 0;

        public AIState aiState;

        //I do NOT like how he named shit here, I'll redo it when I get a chance.
        delegate void EveryFrame();
        EveryFrame everyFrame;
        delegate void LateFrame();
        LateFrame lateFrame;
        delegate void LateLateFrame();
        LateLateFrame latelateFrame;

        [HideInInspector]
        public NavMeshAgent agent;
        [HideInInspector]
        public AIStats aiStats;

        void Start()
        {
            agent = GetComponent<NavMeshAgent>();
            aiStats = GetComponent<AIStats>();
            aiState = AIState.idle;
            ChangeState(AIState.idle);
        }

        void Update()
        {
            MonitorStates();

            if (everyFrame != null)
                everyFrame();

            lFrame_counter++;
            if (lFrame_counter > lFrame)
            {
                if (lateFrame != null)
                    lateFrame();
                //Debug.Log(aiState.ToString());
                lFrame_counter = 0;
            }


            llate_counter++;
            if (llate_counter > llateFrame)
            {
                if (latelateFrame != null)
                    latelateFrame();
                //Debug.Log( "late late frame from" + aiState.ToString());
                llate_counter = 0;
            }
        }

        //Pathfinding shit is going to be added here eventually.
        void MonitorStates()
        {
            switch (aiState)
            {
                case AIState.idle:
                    if (distance < radius)
                        ChangeState(AIState.inRadius);
                    if (distance > maxDistance)
                        ChangeState(AIState.lateIdle);
                    break;
                case AIState.lateIdle:
                    if (distance < maxDistance)
                        ChangeState(AIState.idle);
                    break;
                case AIState.inRadius:
                    if (distance > radius)
                        ChangeState(AIState.idle);
                    if (isClear)
                        ChangeState(AIState.inView);
                    break;
                case AIState.inView:
                    if (distance > radius)
                        ChangeState(AIState.idle);
                    break;
                default:
                    break;
            }
        }

        public void ChangeState(AIState targetState)
        {
            aiState = targetState;
            everyFrame = null;
            lateFrame = null;
            latelateFrame = null;

            switch (targetState)
            {
                case AIState.idle:
                    lateFrame = IdleBehaviours;
                    break;
                case AIState.lateIdle:
                    latelateFrame = IdleBehaviours;
                    break;
                case AIState.inRadius:
                    lateFrame = InRadiusBehaviours;
                    break;

                case AIState.inView:
                    lateFrame = InRadiusBehaviours;
                    latelateFrame += MonitorTargetPosition;
                    everyFrame += InViewBehaviours;
                    lastKnownPosition = playerTarget.position; // Will help cut down the number of times a new path is created.
                    MoveToPosition(lastKnownPosition); //If we disable this, the AI will maintain the patrol but constantly watch the player.
                    break;
                default:
                    break;
            }
        }

        void IdleBehaviours()
        {
            if (playerTarget == null)
                return;
            DistanceCheckPlayer(playerTarget); //Modify this if we have multiple targets.
            CommonBehaviours.PatrolBehaviour(this);
        }


        void InRadiusBehaviours()
        {
            if (playerTarget == null)
                return;

            DistanceCheckPlayer(playerTarget);
            AngleCheck();
            FindDirection(playerTarget);
            isClearView(playerTarget);
            CommonBehaviours.PatrolBehaviour(this);
        }

        void InViewBehaviours()
        {
            if (playerTarget == null)
                return;

            FindDirection(playerTarget);
            RotateTowardsTarget();
        }

        void DistanceCheckPlayer(Transform target)
        {
            //Debug.Log("distance");
            distance = Vector3.Distance(transform.position, target.position);
        }

        void isClearView(Transform target)
        {
            isClear = false;
            //Here's your fucking raycast.
            RaycastHit hit;
            Vector3 origin = transform.position;
            if (Physics.Raycast(origin, direction, out hit, radius)) //Made to be the same as our radius.
            {
                //Shit ain't going to start rotating until a raycast hits the desired target.
                if (hit.transform.CompareTag("Player"))
                {
                    isClear = true;
                }
            }
        }

        void AngleCheck()
        {
            rotDirection = direction;
            rotDirection.y = 0; //Ensures the y direction of the AI rotation is accurate
            if (rotDirection == Vector3.zero)
                rotDirection = transform.forward;

            float angle = Vector3.Angle(transform.forward, rotDirection); //This deals with the cone of vision.

            isInAngle = (angle < 45);
        }

        void RotateTowardsTarget()
        {
            //A bit of optimization, helps with AI path recalculation. Especially with multiple AI.
            if (rotDirection == Vector3.zero)
                rotDirection = transform.forward;

            Quaternion targetRotation = Quaternion.LookRotation(rotDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5);
            //This is just to show that the AI can see the target.
            //Difference between having a target and not having one.
        }

        void FindDirection(Transform target)
        {
            direction = target.position - transform.position;
            rotDirection = direction;
            rotDirection.y = 0;
        }



        //Helps determine the last known position of the player.
        void MonitorTargetPosition()
        {
            float delta_distance = Vector3.Distance(playerTarget.position, lastKnownPosition);
            if (delta_distance > 2)
            {
                lastKnownPosition = playerTarget.position;
                MoveToPosition(lastKnownPosition);
            }
        }

        //Note that by having it only update once, the AI will only follow once it updates (not a constant follow)
        public void MoveToPosition(Vector3 targetPosition)
        {
            agent.SetDestination(targetPosition); //But by only updating once (as opposed to every frame) it is much less resource intensive.
        }

        //Obsolete. Need to use isStopped.
        /*
        public bool StopMoving()
        {
            agent.Stop(true);
        }
        */

        //Beginning of a finite state machine. This is currently unoptimized.
        public enum AIState
        {
            idle, lateIdle, inRadius, inView

        }
    }
}