using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Damage : MonoBehaviour
{
    public float damagePerSecond = 5f, health = 100f;
    private FieldOfView fieldOfView;

    public GameObject endGameUI;

    public TextMeshProUGUI winLoseText, endGameText;

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
        
        if (gameObject.CompareTag("Player") && health <= 0)
        {
            Time.timeScale = 0f;
            endGameUI.SetActive(true);
            winLoseText.text = "You Lose!";
            endGameText.text = "You have be trapped inside the Shadow Realm";
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
