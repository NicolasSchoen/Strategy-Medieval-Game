using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraToHead : MonoBehaviour
{
    personController PC;
    private Transform Neck;
    private GameObject PlayerHealthBar;
    //private spineRotation Spine;

    private void Start()
    {
        PC = transform.root.GetComponent<personController>();
        Neck = transform.parent;
        PlayerHealthBar = transform.root.GetChild(3).gameObject;
        //Spine = transform.parent.parent.parent.parent.GetComponent<spineRotation>();
    }

    private void Update()
    {
        if (PC.isControlled)
        {
            Neck.localScale = new Vector3(0.1f, 0.1f, 0.1f);
            PlayerHealthBar.SetActive(false);
            //Spine.enabled = true;
        }
        else
        {
            Neck.localScale = new Vector3(1f, 1f, 1f);
            PlayerHealthBar.SetActive(true);
            //Spine.enabled = false;
        }
    }

    void LateUpdate()
    {
        transform.root.GetComponentInChildren<Camera>().transform.parent.transform.position = transform.position;
    }
}
