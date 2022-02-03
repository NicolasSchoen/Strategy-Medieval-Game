using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class dayLight : MonoBehaviour
{
    private TextMeshProUGUI dayCountText;
    private TextMeshProUGUI timeText;

    public Material skyboxDay;
    public Material skyboxNight;

    [Header("Sun")]
    public float changespeed = 1f;

    [HideInInspector]
    public bool isNight = false;
    public GameObject nightLight;
    public bool useDayNightCycle = true;

    [Header("Weather Effect")]
    public GameObject weather;
    [SerializeField] private int RNG;
    private int time = 10;
    private int dayCounter = 1;

    [Header("Day-Night Change")]
    public Color dayColor = new Color(0.5019608f, 0.4941176f, 0.454902f);
    public Color nightColor = Color.black;
    public float nightLightIntensity = 1f;
    private float changeTime = 45f;
    private float t = 0;
    [Range(0f, 35f)] public float shorterNight = 35f;

    void Start()
    {
        RenderSettings.ambientLight = dayColor;
        changeTime /= changespeed;

        if (!useDayNightCycle)
        {
            this.enabled = false;
        }

        dayCountText = GameObject.Find("TextDayCount").GetComponent<TextMeshProUGUI>();
        dayCountText.text = "Day 1";

        timeText = GameObject.Find("TextTime").GetComponent<TextMeshProUGUI>();

        InvokeRepeating("TimeDisplay", 0f, (((360f - (shorterNight * 2f)) / 24f) / changespeed));
    }

    void Update()
    {
        transform.Rotate(Vector3.right * changespeed * Time.deltaTime);

        if (!isNight && transform.rotation.eulerAngles.x > 180)
        {
            Debug.Log("changing to Night");            
            isNight = true;
            //RenderSettings.skybox = skyboxNight;            
        }
        if (transform.rotation.eulerAngles.x < 305 && transform.rotation.eulerAngles.x > 300)        //Kürzere Nacht
        {
            transform.Rotate(shorterNight, 0, 0);
        }
        if (time >= 20)
        {            
            RenderSettings.ambientLight = Color.Lerp(dayColor, nightColor, t);
            GetComponent<Light>().intensity = 1 - t;
            nightLight.GetComponent<Light>().intensity = t * nightLightIntensity;
            //RenderSettings.fogDensity = 0.01f - (t / 100);
            if (t < 1)
            {
                t += Time.deltaTime / changeTime;
            }
        }
        if (isNight && transform.rotation.eulerAngles.x < 180)
        {            
            Debug.Log("changing to Day");            
            isNight = false;
            //RenderSettings.skybox = skyboxDay;

            dayCounter++;
            dayCountText.text = "Day " + dayCounter;
            gameStatistic.GS.nextDay();

            if (weather.activeSelf)
            {
                Debug.Log("Rain off");
                weather.SetActive(false);
            }

            RNG = Random.Range(0, (int)(100 / globalVariables.possibilityForRain));
            if (RNG == 0)            //Regen wird für einen Tag aktiviert wenn die Zufallszahl getroffen wird
            {
                Debug.Log("Rain on");
                weather.SetActive(true);
            }
        }
        if (time > 6 && time <= 10)
        {            
            RenderSettings.ambientLight = Color.Lerp(dayColor, nightColor, t);
            GetComponent<Light>().intensity = 1 - t;
            nightLight.GetComponent<Light>().intensity = t * nightLightIntensity;
            //RenderSettings.fogDensity = 0.01f - (t / 100);
            if (t > 0)
            {
                t -= Time.deltaTime / changeTime;
            }
        }
    }

    void TimeDisplay()
    {
        timeText.text = time + " o'clock";
        time++;
        time %= 24;
    }
}
