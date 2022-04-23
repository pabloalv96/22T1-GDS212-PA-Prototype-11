using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class AstarAI : MonoBehaviour
{
    public Transform targetPosition;
    private CharacterController controller;

    public Path path;

    public float speed = 2f;

    public float nextWaypointDistance = 2.5f;

    private int currentWaypoint = 0;

    public bool reachedEndOfPath;
    void Start()
    {
        Seeker seeker = GetComponent<Seeker>();

        controller = GetComponent<CharacterController>();

        seeker.StartPath(transform.position, targetPosition.position, OnPathComplete);
    }

   public void OnPathComplete (Path p)
    {
        Debug.Log("Path found! Errors:" + p.error);

        if (!p.error)
        {
            path = p;
            // Reset the waypoint counter so that we start to move towards the first point in the path
            currentWaypoint = 0;
        }
    }

    public void Update()
    {
        if (path == null)
        {
            // if there isn't a path, dont do anything
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

                    reachedEndOfPath = true;
                    break;
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
    }
}
