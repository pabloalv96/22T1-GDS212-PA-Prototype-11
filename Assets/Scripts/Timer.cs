using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Timer : MonoBehaviour
{
    [SerializeField] float timeValue = 90f;
    [SerializeField] TextMeshProUGUI timeText, winLoseText, endGameText;
    public GameObject endGameUI;

    // Start is called before the first frame update
    void Start()
    {
        endGameUI.SetActive(false);
        Time.timeScale = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        if(timeValue > 0)
        {
            timeValue -= Time.deltaTime;
        } else
        {
            timeValue = 0;
        }

        DisplayTime(timeValue); 

        if (timeValue <= 0)
        {
            Time.timeScale = 0f;
            endGameUI.SetActive(true);
            winLoseText.text = "You Win!";
            endGameText.text = "You have ascended into the light!";
        }
    }

    void DisplayTime(float timeToDisplay)
    {
        if(timeToDisplay < 0)
        {
            timeToDisplay = 0;
        }

        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);

        timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }


}
