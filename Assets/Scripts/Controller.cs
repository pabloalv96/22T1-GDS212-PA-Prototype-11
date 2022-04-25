using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{

    public float moveSpeed = 6, health = 100f;

    Rigidbody rb;
    Camera viewCamera;
    Vector3 velocity;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        viewCamera = Camera.main;

        GetComponent<MeshRenderer>().material.color = Random.ColorHSV(0f, 1f, 1f, 1f, 0.9f, 1f);

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mousePos = viewCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, viewCamera.transform.position.y));
        transform.LookAt(mousePos + Vector3.up * transform.position.y);
        velocity = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized * moveSpeed;
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + velocity * Time.fixedDeltaTime);
    }
}
