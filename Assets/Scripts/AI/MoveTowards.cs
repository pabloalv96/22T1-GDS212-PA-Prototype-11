using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime;

public class MoveTowards : Action
{
    public float speed = 5f;
    public SharedTransform target;

    //private CharacterController controller;

    public override void OnAwake()
    {
        //controller = GetComponent<CharacterController>();
    }
    public override TaskStatus OnUpdate()
    {
        if (Vector3.SqrMagnitude(transform.position - target.Value.position) < 0.1f)
        {
            return TaskStatus.Success;
        }
        //transform.position = Vector3.MoveTowards(transform.position, target.Value.position, speed * Time.deltaTime);

        // get and normalize the direction to the next waypoint to 1 world unit
        Vector3 dir = (target.Value.position - transform.position).normalized;
        // multiply the direction by the desired speed
        Vector3 velocity = dir * speed;

        transform.position += velocity * Time.deltaTime;

        transform.LookAt(dir);

        return TaskStatus.Running;
    }
}
