using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class planetCamera : MonoBehaviour
{
    public GameObject level1;
    public GameObject level2;
    public GameObject level3;
    public GameObject level4;
    public GameObject level5;

    public Material placementGood;
    public Material placementBad;
    // Start is called before the first frame update
    void Start()
    {
        level2.GetComponentInChildren<Renderer>().material = placementBad;
        level3.GetComponentInChildren<Renderer>().material = placementBad;
        level4.GetComponentInChildren<Renderer>().material = placementBad;
        level5.GetComponentInChildren<Renderer>().material = placementBad;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void backToMenu()
    {
        SceneManager.LoadScene("menu");
    }
}
