using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage : MonoBehaviour
{
    public float damagePerSecond = 5f, health = 100f;
    private FieldOfView fieldOfView;
    void Start()
    {
        fieldOfView = GetComponent<FieldOfView>();
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, fieldOfView.viewRadius))
        {
            if (hit.transform.CompareTag("AI"))
            {
                hit.transform.GetComponent<Damage>().health -= damagePerSecond * Time.deltaTime;
            }

            if (hit.transform.CompareTag("Player"))
            {
                hit.transform.GetComponent<Damage>().health -= damagePerSecond * Time.deltaTime;
            }
        }

        if (gameObject.CompareTag("AI") && health <= 0)
        {
            Destroy(gameObject);
        }
    }

    //public void OnTriggerEnter(Collider other)
    //{
    //    if (other.gameObject.CompareTag("AI"))
    //    {
    //        other.transform.GetComponent<AIPatrol>().health -= damagePerSecond * Time.deltaTime;
    //    }

    //    if (other.gameObject.CompareTag("Player"))
    //    {
    //        other.transform.GetComponent<Controller>().health -= damagePerSecond * Time.deltaTime;
    //    }
    //}
}
