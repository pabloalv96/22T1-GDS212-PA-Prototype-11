using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyAI : MonoBehaviour
{
    public Seeker seeker;
    public CharacterController agent;

    public Path path;

    public Transform targetPos;

    public LayerMask whatIsGround, whatIsPlayer;

    public float nextWaypointDistance = 3;

    private int currentWaypoint = 0;

    public bool reachedEndOfPath;

    //Patroling
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    //States
    public float speed, sightRange;
    public bool playerInSightRange;


    private void Awake()
    {
        //player = GameObject.Find("Player").transform;
        agent = GetComponent<CharacterController>();
        seeker = GetComponent<Seeker>();
    }


    private void Update()
    {
        if (path == null)
        {
            return;
        }

        reachedEndOfPath = false;
        float distanceToWaypoint;

        while (true)
        {
            distanceToWaypoint = Vector3.Distance(transform.position, path.vectorPath[currentWaypoint]);
            if (distanceToWaypoint < nextWaypointDistance)
            {
                // Check if there is another waypoint or if we have reached the end of the path
                if (currentWaypoint + 1 < path.vectorPath.Count)
                {
                    currentWaypoint++;
                }
                else
                {

                    // You can use this to trigger some special code if your game requires that.
                    reachedEndOfPath = true;
                    break;
                }
            }
            else
            {
                break;
            }
        }

        Vector3 dir = (path.vectorPath[currentWaypoint] - transform.position).normalized;
        Vector3 velocity = dir * speed;

        agent.SimpleMove(velocity);

        //Check for sight range
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);


        if (!playerInSightRange) Patrolling();
        if (playerInSightRange && CoinFlip()) ChasePlayer();
        if (playerInSightRange && !CoinFlip()) RunAway();

    }

    private bool CoinFlip()
    {
        int r = Random.Range(0, 2);

        if (r == 0)
        {
            return true;
        }
        else return false;
    }

    private void Patrolling()
    {
        if (!walkPointSet) SearchWalkPoint();

        if (walkPointSet)
            seeker.StartPath(transform.position, walkPoint, OnPathComplete);

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        //walk point reached
        if (distanceToWalkPoint.magnitude < 1f)
            walkPointSet = false;
    }

    public void OnPathComplete(Path p)
    {
        Debug.Log("A path was calculated. Did it fail with an error? " + p.error);

        if (!p.error)
        {
            path = p;
            // Reset the waypoint counter so that we start to move towards the first point in the path
            currentWaypoint = 0;
        }
    }

    private void SearchWalkPoint()
    {
        // calculate random point in range
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        walkPointSet = true;
    }

    private void ChasePlayer()
    {
        seeker.StartPath(transform.position, targetPos.position, OnPathComplete);
    }

    private void RunAway()
    {
        Vector3 fleePosition = new Vector3(-targetPos.position.x, transform.position.y, -targetPos.position.z);


    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
}


