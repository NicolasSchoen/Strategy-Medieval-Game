using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    private Image healthBarTop;
    public bool isEnemy = false;

    // Start is called before the first frame update
    void Start()
    {
        healthBarTop = transform.GetChild(0).transform.GetChild(0).GetComponent<Image>();
    }

    void Update()
    {
        if (isEnemy)
        {
            healthBarTop.fillAmount = transform.root.GetComponent<enemyAttributes>().health / transform.root.GetComponent<enemyAttributes>().maxhealth;
        }
        else
        {
            healthBarTop.fillAmount = transform.root.GetComponent<personAttributes>().health / transform.root.GetComponent<personAttributes>().maxhealth;
        }
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if(Camera.main != null)
        {
            //if (Camera.main.GetComponent<cameraControl>().tabletopCam.enabled)
            //{
                transform.LookAt(Camera.main.transform);
                transform.Rotate(0, 180, 0);
            //}
        }            
    }
}
