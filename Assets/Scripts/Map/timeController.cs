using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class timeController : MonoBehaviour
{
    public int timeSpeedIndex = 2;
    public float[] timeScales = {0f,.5f,1f,2f,5f,10f };

    public GameObject[] buttons = new GameObject[6];
    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = timeScales[timeSpeedIndex];
        buttons[timeSpeedIndex].GetComponent<Image>().color = Color.grey;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Comma))
        {
            //decrease speed
            if (timeSpeedIndex > 0) timeSpeedIndex--;
            Time.timeScale = timeScales[timeSpeedIndex];
            updateTimeCanvas();
        }
        if (Input.GetKeyDown(KeyCode.Period))
        {
            //increase speed
            if (timeSpeedIndex < timeScales.Length-1) timeSpeedIndex++;
            Time.timeScale = timeScales[timeSpeedIndex];
            updateTimeCanvas();
        }
    }

    public void setTimeScale(int index)
    {
        if (index < 0 || index >= timeScales.Length) return;
        timeSpeedIndex = index;
        Time.timeScale = timeScales[timeSpeedIndex];
        updateTimeCanvas();
    }

    public void updateTimeCanvas()
    {
        foreach (GameObject button in buttons)
        {
            button.GetComponent<Image>().color = Color.white;
        }

        buttons[timeSpeedIndex].GetComponent<Image>().color = Color.grey;
    }
}
