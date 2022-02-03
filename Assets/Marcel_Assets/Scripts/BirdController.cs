using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdController : MonoBehaviour
{
    public float speed = 10.0f;
    private Camera birdCam;
    public Camera tabletopCam;

    private bool birdCamActive = false;

    private Vector3 moveDirection;
    private CharacterController controller;

    private void Awake()
    {
        moveDirection = Vector3.zero;
        controller = GetComponent<CharacterController>();
    }

    // Start is called before the first frame update
    void Start()
    {
        birdCam = GetComponentInChildren<Camera>();
    }

    // Update is called once per frame
    void Update()
    {        
        if (Input.GetKeyDown(KeyCode.J))
        {
            tabletopCam.enabled = false;
            birdCamActive = !birdCamActive;
            controller.enabled = true;
        }

        if (birdCamActive) {
            birdCam.enabled = true;
            BirdFPS();
        }
        else
        {
            birdCam.enabled = false;
            TableTop();
        }
    }

    void BirdFPS()
    {
        float h = -Input.GetAxis("Mouse Y");
        float v = Input.GetAxis("Mouse X");

        Vector3 eulers = transform.localEulerAngles;

        eulers.x += h;
        eulers.y += v;
        eulers.z = 0;

        transform.localEulerAngles = eulers;

        moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));


        moveDirection = transform.TransformDirection(moveDirection);
        moveDirection *= speed;

        controller.Move(moveDirection * Time.deltaTime);

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        GetComponent<birdMovement>().enabled = false;
    }

    void TableTop()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        birdCam.enabled = false;
        tabletopCam.enabled = true;

        birdCamActive = false;

        GetComponent<birdMovement>().enabled = true;
        controller.enabled = false;

        Vector3 temp = transform.rotation.eulerAngles;
        temp.x = 0f;
        transform.rotation = Quaternion.Euler(temp);
    }
}
