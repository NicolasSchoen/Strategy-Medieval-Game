using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour
{
    private Animator gateAnim;    
    // Start is called before the first frame update
    void Start()
    {
        gateAnim = GetComponentInChildren<Animator>();        
    }

    public void interactWithObject(GameObject other)
    {
        if (gateAnim != null) gateAnim.SetTrigger("GateInteraction");        
    }   
}
