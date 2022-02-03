using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spineRotation : MonoBehaviour
{
    void LateUpdate()
    {
        transform.rotation = transform.root.GetComponentInChildren<Camera>().transform.rotation;
    }
}
