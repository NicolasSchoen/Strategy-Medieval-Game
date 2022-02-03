using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenucameraController : MonoBehaviour
{
    public float rotationSpeed = 20f;
    public float ascendingSpeed = 1f;
    public float maxHeight = 30f;
    public float minHeight;
    // Start is called before the first frame update
    void Start()
    {
        minHeight = transform.parent.position.y;
        setNewMapPosition();
    }

    public void setNewMapPosition()
    {
        transform.parent.parent.position = new Vector3(globalVariables.mapSize / 2, 5, globalVariables.mapSize / 2);
    }

    // Update is called once per frame
    void Update()
    {
        transform.parent.parent.Rotate(0, rotationSpeed * Time.unscaledDeltaTime, 0);
        transform.parent.Translate(0, ascendingSpeed * Time.unscaledDeltaTime, -ascendingSpeed * Time.unscaledDeltaTime);
        if (transform.parent.position.y > maxHeight) ascendingSpeed = -ascendingSpeed;
        if (transform.parent.position.y < minHeight) ascendingSpeed = -ascendingSpeed;
    }
}
