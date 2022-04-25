using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime;

public class WithinSight : Conditional
{
    //public float fieldOfViewAngle = 45f, lookDistance = 5f;
    public string targetTag;
    public SharedTransform target;
    public FieldOfView fieldOfView;

    private Transform[] possibleTargets;

    public override void OnAwake()
    {
        var targets = GameObject.FindGameObjectsWithTag(targetTag);
        possibleTargets = new Transform[targets.Length];
        for (int i = 0; i < targets.Length; i++)
        {
            possibleTargets[i] = targets[i].transform;
        }
    }

    public override TaskStatus OnUpdate()
    {
        for (int i = 0; i < possibleTargets.Length; i++)
        {
            if (withinSight(possibleTargets[i], fieldOfView.viewAngle, fieldOfView.viewRadius))
            {
                target.Value = possibleTargets[i];
                return TaskStatus.Success;
            }
        }
        return TaskStatus.Failure;
    }

    public bool withinSight(Transform targetTransform, float fieldOfViewAngle, float fieldOfViewRadius)
    {
        Vector3 direction = targetTransform.position - transform.position;

        // An object is within sight if the angle is less than the field of view and closer than the lookDistance
        if (Vector3.Angle(direction, transform.forward) < fieldOfViewAngle && Vector3.Distance(targetTransform.position, transform.position) < fieldOfView.viewRadius)
        {
            return true;
        }
        else return false;
    }

}
