using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class AIPatrol : MonoBehaviour
{
    public Transform targetPosition, eyes, patrolTarget, fleeTarget;

    private Seeker seeker;

    private CharacterController controller;

    public Path path;

    public float speed = 2f, turnSpeed = 5f, health = 100f;

    public float nextWaypointDistance = 2.5f;

    private int currentWaypoint = 0;

    public float repathRate = 0.5f;
    private float lastRepath = float.NegativeInfinity;

    public bool reachedEndOfPath;

    // detection variables

    public List<GameObject> detectedObjects;

    public FieldOfView fieldOfView;

    public void Start()
    {
        seeker = GetComponent<Seeker>();

        controller = GetComponent<CharacterController>();

        patrolTarget.GetComponent<RandomWaypoint>().SetNewWaypoint();

        targetPosition = patrolTarget;

        detectedObjects = new List<GameObject>();

        GetComponent<MeshRenderer>().material.color = Random.ColorHSV(0f, 1f, 1f, 1f, 0.9f, 1f);



    }

    public void OnPathComplete (Path p)
    {
        Debug.Log("Path found! Errors:" + p.error);
        
        p.Claim(this);
        if (!p.error)
        {
            if (path != null) path.Release(this);
            {
                path = p;
                // Reset the waypoint counter so that we start to move towards the first point in the path
                currentWaypoint = 0;
            } 
        }
        else
        {
            p.Release(this);
        }
    }

    public List<GameObject> DetectObjects(List<GameObject> objectsAroundAI)
    {

        RaycastHit hit;

        if (Physics.SphereCast(eyes.position, fieldOfView.viewRadius, transform.forward, out hit, fieldOfView.viewRadius))
        {
            Debug.DrawLine(transform.position, hit.point, Color.cyan);

            if (!detectedObjects.Contains(hit.transform.gameObject))
                objectsAroundAI.Add(hit.transform.gameObject);
        }

         for (int i = 0; i < objectsAroundAI.Count; i++)
            {
                if (objectsAroundAI[i] != Physics.SphereCast(eyes.position, fieldOfView.viewRadius, transform.forward, out hit, fieldOfView.viewRadius))
                {
                    objectsAroundAI.Remove(detectedObjects[i]);
                }

            if (objectsAroundAI[i].transform.CompareTag("AI") || objectsAroundAI[i].transform.CompareTag("Player"))
            {
                int r = Random.Range(0, 2);
                
                switch (r)
                {
                    case 0:
                        {
                            targetPosition = objectsAroundAI[i].transform;
                            transform.LookAt(targetPosition);
                            Debug.Log("Chasing Target");
                            break;
                        }
                    case 1:
                        {
                            fleeTarget.position = new Vector3(objectsAroundAI[i].transform.position.x, transform.position.y, -objectsAroundAI[i].transform.position.z);
                            targetPosition = fleeTarget;
                            transform.LookAt(fleeTarget);

                            Debug.Log("Fleeing");

                            break;
                        }
                    //case 2:
                    //    {
                    //        targetPosition = patrolTarget;
                    //        break;
                    //    }
                }

            }
            else targetPosition = patrolTarget;

        }

        return objectsAroundAI;
    }

    public bool CoinFlip()
    {
        int r = Random.Range(0, 2);

        if (r == 0)
        {
            return true;
        }
        else return false;
    }

    public void Update()
    {
        DetectObjects(detectedObjects);

       if (Time.time > lastRepath + repathRate && seeker.IsDone())
        {
            lastRepath = Time.time;

            seeker.StartPath(transform.position, targetPosition.position, OnPathComplete);

        }

        if (path == null)
        {
            // if there isn't a path, dont do anything
            // return TaskStatus.Failure;
            return;
        }

        // loop to check whether it has reached the current waypoint and should swap to the next

        reachedEndOfPath = false;

        float distanceToWaypoint;

        while (true)
        {
            distanceToWaypoint = Vector3.Distance(transform.position, path.vectorPath[currentWaypoint]);
            if (distanceToWaypoint < nextWaypointDistance)
            {
                // check if it has reached the end of the path
                if (currentWaypoint + 1 < path.vectorPath.Count)
                {
                    currentWaypoint++;
                }
                else
                {
                    targetPosition.GetComponent<RandomWaypoint>().SetNewWaypoint();
                    reachedEndOfPath = true;
                    break;
                   // return TaskStatus.Success;
                }
            }
            else
            {
                break;
            }
        }

        //smoothly slow down upon approaching ther end of the path
        //value will go from 1 to 0
        var speedFactor = reachedEndOfPath ? Mathf.Sqrt(distanceToWaypoint / nextWaypointDistance) : 1f;

        // get and normalize the direction to the next waypoint to 1 world unit
        Vector3 dir = (path.vectorPath[currentWaypoint] - transform.position).normalized;
        // multiply the direction by the desired speed
        Vector3 velocity = dir * speed * speedFactor;

        //Move the agent with the Character Controller
        controller.SimpleMove(velocity);
        // Note that SimpleMove takes a velocity in meters/second, so we should not multiply by Time.deltaTime

        //transform.position += velocity * Time.deltaTime;

        //transform.LookAt(targetPosition.position);
        transform.LookAt(targetPosition.position * turnSpeed * Time.deltaTime);


    }
}
