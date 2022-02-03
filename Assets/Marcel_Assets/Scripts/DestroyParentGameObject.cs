using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyParentGameObject : MonoBehaviour
{
    void DestroyObject()
    {
        Destroy(this.gameObject.transform.parent.gameObject);
    }
}
