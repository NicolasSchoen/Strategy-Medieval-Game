using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomGround : MonoBehaviour
{
    public Material[] groundMaterial;
    private int RNG;

    // Start is called before the first frame update
    void Start()
    {       
        RNG = Random.Range(0, groundMaterial.Length);

        GameObject.Find("ground").GetComponent<MeshRenderer>().material = groundMaterial[RNG];
    }
}
