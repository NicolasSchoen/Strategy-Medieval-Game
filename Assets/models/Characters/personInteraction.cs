using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class personInteraction : MonoBehaviour
{
    public List<GameObject> nearbyObjects;
    //public Canvas fpsCanvas;
    private TextMeshProUGUI interactionText;
    private bool isControlled = false;
    // Start is called before the first frame update
    void Start()
    {
        nearbyObjects = new List<GameObject>();
        //interactionText = fpsCanvas.GetComponent<TextMeshProUGUI>();
        interactionText = GameObject.Find("TextInteraction").GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isControlled)
        {
            if (Input.GetKeyDown(KeyCode.E) && gameObject.GetComponent<personController>().isControlled)
            {
                //print nearby objects
                /*foreach (GameObject nearbyObject in nearbyObjects)
                {
                    Debug.Log(nearbyObject.name);
                    //interact with close object here
                    nearbyObject.SendMessageUpwards("interactWithObject", gameObject);
                }*/
                InteractWithNearestObject();
            }

            if (nearbyObjects.Count > 0)
            {
                if (nearbyObjects[0] == null)
                {
                    nearbyObjects.RemoveAt(0);
                    interactionText.text = "";
                }
                else
                {
                    try
                    {
                        interactionText.text = nearbyObjects[0].GetComponentInParent<ModelAttributes>().interactiontext;//"press E to interact with " + nearbyObjects[0].name;
                    }
                    catch (System.Exception)
                    {
                        interactionText.text = "";
                    }

                    /*if (Input.GetMouseButtonDown(1))
                    {
                        nearbyObjects[0].SendMessageUpwards("hitObject");
                    }*/
                }
            }
        } 
    }

    public void InteractWithNearestObject()
    {
        if (nearbyObjects.Count > 0)
        {
            if(nearbyObjects[0].tag == "building_civilian")
            {
                if(nearbyObjects[0].GetComponentInParent<ModelAttributes>().buildingName == "Butcher")
                {
                    //eat
                    GetComponent<personController>().eatFood();
                    if (nearbyObjects[0].GetComponentInParent<AudioSource>() != null) nearbyObjects[0].GetComponentInParent<AudioSource>().Play();
                }
                if (nearbyObjects[0].GetComponentInParent<ModelAttributes>().buildingName == "Blacksmith")
                {
                    //restore armor
                    GetComponent<personController>().repairArmor();
                    if(nearbyObjects[0].GetComponentInParent<AudioSource>() != null) nearbyObjects[0].GetComponentInParent<AudioSource>().Play();
                }
            }
            else
            {
                nearbyObjects[0].SendMessageUpwards("interactWithObject", gameObject);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 2) return;    //if only trigger collider
        if(other.tag != "ground" && other.tag != "Untagged" && other.tag != "nature")
        {
            nearbyObjects.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 2) return;    //if only trigger collider
        if (other.tag != "ground" && other.tag != "Untagged" && other.tag != "nature")
        {
            nearbyObjects.Remove(other.gameObject);
            interactionText.text = "";
        }
    }

    public void resetNearObjectsList()
    {
        nearbyObjects.Clear();
        interactionText.text = "";
    }

    public void setControlled(bool value)
    {
        isControlled = value;
    }
}
