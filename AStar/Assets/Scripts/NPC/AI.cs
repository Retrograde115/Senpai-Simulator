using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Pathfinding;
using GridMaster;

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

    Pathfinding.PathfindMaster path;
    Node currNode; //Current node.
    List<Node> currPath;

    NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        path = Pathfinding.PathfindMaster.GetInstance();
        aiState = AIState.idle;
        ChangeState(AIState.idle);

        //Sets the start position of the AI on the grid.
        GridMaster.GridBase grid = GridMaster.GridBase.GetInstance();
        currNode = grid.GetNode(0, 0, 0);
        transform.position = currNode.nRef.vis.transform.position;
    }

    void Update()
    {
        MonitorStates();

        if (everyFrame != null)
            everyFrame();

        lFrame_counter++;
        if(lFrame_counter > lFrame)
        {
            if (lateFrame != null)
                lateFrame();
            //Debug.Log(aiState.ToString());
            lFrame_counter = 0;
        }


        llate_counter++;
        if(llate_counter > llateFrame)
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
        everyFrame = MonitorPosition;

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

            //Anything commented here was for the Navmesh which will be implemented later.
            case AIState.inView:
                lateFrame = InRadiusBehaviours;
                //latelateFrame += MonitorTargetPosition;
                latelateFrame = MonitorPlayerPosition;
                everyFrame += InViewBehaviours;
                everyFrame += MovementActual;
                // = playerTarget.position; // Will help cut down the number of times a new path is created.
                //MoveToPosition(lastKnownPosition);
                lastKnownPosition = playerTarget.position;
                MoveToTarget(playerTarget.position);
                break;
            default:
                break;
        }
    }

    void IdleBehaviours()
    {
        if (playerTarget == null)
            return;
        DistanceCheck(playerTarget); //Modify this if we have multiple targets.
    }


    void InRadiusBehaviours()
    {
        if (playerTarget == null)
            return;

        DistanceCheck(playerTarget);
        AngleCheck();
        FindDirection(playerTarget);
        isClearView(playerTarget);
    }

    void InViewBehaviours()
    {
        if (playerTarget == null)
            return;

        FindDirection(playerTarget);
        RotateTowardsTarget();
    }

    void DistanceCheck(Transform target)
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

    //Note that by having it only update once, the AI will only follow once it updates (not a constant follow)
    void MoveToPosition(Vector3 targetPosition)
    {
        agent.SetDestination(targetPosition); //But by only updating once (as opposed to every frame) it is much less resource intensive.
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

    //AI pathing across the grid
    void MoveToTarget(Vector3 position)
    {
        //Determines the end (target) node.
        Node targetNode = GridBase.GetInstance().GetNodeFromVector3(position);

        Pathfinding.PathfindMaster.GetInstance().RequestPathfind(currNode, targetNode, PathfindCallback);
    }

    //AI finding out about its path.
    //Slightly quick and dirty.
    void PathfindCallback(List<Node> path)
    {
        currPath = new List<Node>();
        currPath = path;
        currPath.AddRange(path);
        EvaluatePath();
        pathIndex = 0;
        initLerp = false;
    }

    int pathIndex;
    float pathT;

    bool initLerp;
    Vector3 startPosition;
    Vector3 targetPosition;
    float distanceToTarget;
    float speed = 1;

    void MovementActual()
    {
        if (shortPath == null)
            return;

        if (pathIndex > shortPath.Count - 1)
            return;

        if(!initLerp)
        {
            startPosition = transform.position;
            targetPosition = shortPath[pathIndex].nRef.vis.position;
            distanceToTarget = Vector3.Distance(startPosition, targetPosition);
            initLerp = true;
            pathT = 0;
        }

        //Adjusts the rate at which the AI will approach the player.
        float targetSpeed = Time.deltaTime * (speed / distanceToTarget);
        pathT += targetSpeed;

        if(pathT > 1)
        {
            initLerp = false;
            pathT = 1;
            pathIndex++;
        }

        transform.position = Vector3.Lerp(transform.position, targetPosition, pathT);

        if(pathT == 1)
        {
             pathT = 0;
        }

    }

    void MonitorPosition()
    {
        //This is the fast and dirty way.
        currNode = GridBase.GetInstance().GetNodeFromVector3(transform.position);
    }

    //Allows the AI to follow the player as long as they are in view.
    void MonitorPlayerPosition()
    {
        float distance = Vector3.Distance(playerTarget.position, lastKnownPosition);
        if(distance > 2)
        {
            currPath = null;
            MonitorPosition();
            lastKnownPosition = playerTarget.position;
            MoveToTarget(lastKnownPosition);
        }
    }

    List<Node> shortPath;

    //Improved pathfinding.
    void EvaluatePath()
    {
        shortPath = new List<Node>();
        Vector3 curDirection = Vector3.zero;
        shortPath.Add(currPath[0]);

        for (int i = 1; i < currPath.Count; i++)
        {
            Vector3 nextDirection = Vector3.zero;
            nextDirection.x = (currPath[i - 1].x - currPath[i].x);
            nextDirection.y = (currPath[i - 1].y - currPath[i].y);
            nextDirection.z = (currPath[i - 1].z - currPath[i].z);

            if (!Vector3.Equals(nextDirection, curDirection))
            {
                shortPath.Add(currPath[i - 1]);
                curDirection = nextDirection;
            }
            else
            {

            }
        }

        shortPath.Add(currPath[currPath.Count - 1]); //Adds final node position.
    }

    //Beginning of a finite state machine. This is currently unoptimized.
    public enum AIState
    {
        idle, lateIdle, inRadius, inView

    }

}