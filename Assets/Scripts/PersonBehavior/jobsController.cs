using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class jobsController : MonoBehaviour
{
    public List<GameObject> fieldsToHarvest = new List<GameObject>();
    public List<GameObject> treesToCut = new List<GameObject>();
    public List<GameObject> rocksToMine = new List<GameObject>();

    public static jobsController JC;
    // Start is called before the first frame update
    void Start()
    {
        JC = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
