using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopDownCamera : MonoBehaviour
{
    //if we want we can change the view of the camaera from orthographic
    //to perspective which allows us to show off more of the car
    [SerializeField] Transform observeable;
    [SerializeField] float aheadSpeed;
    [SerializeField] float followDamping;
    [SerializeField] float cameraHeight;

    Rigidbody _observableRigidBody;

    // Start is called before the first frame update
    void Start()
    {
        //giving the camera a rigid body
        _observableRigidBody = observeable.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (observeable == null)
            return;
        //setting the camera to be in front of the car
        Vector3 targetPosition = observeable.position + Vector3.up * cameraHeight + _observableRigidBody.velocity * aheadSpeed;
        transform.position = Vector3.Lerp(transform.position, targetPosition, followDamping * Time.deltaTime);
    }
}
