using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class personController : MonoBehaviour
{
    private Camera fpsCam;
    private Camera tabletopCam;
    public float walkspeed = 1.5f;
    public float jumpheight = 3.0f;
    public float shiftspeed = 2.0f;
    public float health = 10f;
    public float mouseSensitivity = 100.0f;

    public TextMeshProUGUI txtPersonHealth;
    public TextMeshProUGUI txtPersonArmor;
    public TextMeshProUGUI txtPersonJob;

    [HideInInspector]
    public Image healthBar;
    [HideInInspector]
    public Image armorBar;

    private float speed;

    float horizontal;
    float vertical;
    float mouseX;
    float mouseY;

    float deathHeight = -10;

    public float raylength = 1;

    Quaternion deltaRotation;
    Vector3 deltaPosition;

    Rigidbody rbody;

    Animator anim;

    public float attackRatePerSecond = 1f;
    float nextAttackTime = 0f;

    public bool isControlled = false;
    public bool isDead = false;

    private bool isGrounded = true;

    private GameObject BloodBorderObject;
    private Animator BloodBorderAnim;

    // Start is called before the first frame update
    void Start()
    {
        //gameStatistic.GS.personBorn();

        rbody = GetComponent<Rigidbody>();
        speed = walkspeed;
        fpsCam = GetComponentInChildren<Camera>();        
        tabletopCam = Camera.main;

        anim = GetComponentInChildren<Animator>();

        txtPersonHealth = GameObject.Find("TextHealthP").GetComponent<TextMeshProUGUI>();
        txtPersonArmor = GameObject.Find("TextArmorP").GetComponent<TextMeshProUGUI>();
        txtPersonJob = GameObject.Find("TextJobP").GetComponent<TextMeshProUGUI>();
        healthBar = GameObject.Find("HealthP").GetComponent<Image>();
        armorBar = GameObject.Find("ArmorP").GetComponent<Image>();        

        gameStatistic.GS.personBorn(gameObject);

        BloodBorderObject = GameObject.Find("BloodBorder");
        BloodBorderAnim = BloodBorderObject.GetComponent<Animator>();
    }

    private void OnDestroy()
    {
        gameStatistic.GS.personDied(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y < deathHeight)//if person falls through terrain,, destroy him
        {
            if (fpsCam.enabled) switchToTabletop();
            Destroy(gameObject);
        }

        if(fpsCam.enabled)
        {
            if(mouseY < 0 && (fpsCam.transform.rotation.eulerAngles.x < 80f || fpsCam.transform.rotation.eulerAngles.x > 270f)) fpsCam.transform.Rotate(-Vector3.right * mouseY * mouseSensitivity * Time.unscaledDeltaTime);

            if (mouseY > 0 && (fpsCam.transform.rotation.eulerAngles.x > 280f || fpsCam.transform.rotation.eulerAngles.x < 90f)) fpsCam.transform.Rotate(-Vector3.right * mouseY * mouseSensitivity * Time.unscaledDeltaTime);
            Debug.Log("delta rotation: " + fpsCam.transform.rotation.eulerAngles.x);
            deltaRotation = Quaternion.Euler(Vector3.up * mouseX * mouseSensitivity * Time.unscaledDeltaTime);
            rbody.MoveRotation(rbody.rotation * deltaRotation);

            if (Input.GetButtonDown("Fire2")) //right mouse button (change terrain-increaase)
            {
                if (GetComponent<personAttributes>().type == 1)
                {
                    if (anim != null && anim.GetBool("attack"))
                    {

                    }
                    else
                    {
                        if (anim != null && !anim.GetBool("attack")) anim.SetBool("attack", true);
                        //Vector3 fwd = raycastObject.transform.TransformDirection(Vector3.forward);
                        RaycastHit hit;
                        //Ray ray = transform.GetComponentInChildren<Camera>().ScreenPointToRay(Input.mousePosition);
                        Transform cam = transform.GetComponentInChildren<Camera>().transform;
                        if (Physics.Raycast(cam.position, cam.forward, out hit, raylength*2))
                        {
                            Debug.Log(hit.collider.name);
                            if (hit.collider.tag == "ground")
                            {
                                //change terrain
                                changeTerrain(hit, 1);
                            }
                        }
                    }
                }    
            }

            if (Input.GetButtonDown("Fire1")) //left mouse button (change terrain-decrease)
            {
                //if (anim != null && anim.GetBool("attack"))
                //{

                //}
                //else
                //{
                    //if (anim != null && !anim.GetBool("attack")) anim.SetBool("attack", true);
                    //Vector3 fwd = raycastObject.transform.TransformDirection(Vector3.forward);
                    RaycastHit hit;
                    //Ray ray = transform.GetComponentInChildren<Camera>().ScreenPointToRay(Input.mousePosition);
                    Transform cam = transform.GetComponentInChildren<Camera>().transform;
                    if (Physics.Raycast(cam.position, cam.forward, out hit, raylength))
                    {
                        Debug.Log(hit.collider.name);
                        if (hit.collider.tag == "ground")
                        {
                            //change terrain
                            if(GetComponent<personAttributes>().type == 1) changeTerrain(hit, -1);
                        }
                        if (hit.collider.tag == "nature")
                        {
                            //hitNatureObject(hit);
                        }
                    }
                    if (Time.time >= nextAttackTime)         //Anrgiffe auf 1 Angriff pro Sekunde reduzieren
                    {
                        if (anim != null) anim.SetTrigger("attack");
                        nextAttackTime = Time.time + 1f / attackRatePerSecond;
                    }
                //}               
            }

            if (Input.GetKeyDown("c"))
            {
                switchToTabletop();
            }

            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                speed = shiftspeed * walkspeed;
            }
            if (Input.GetKeyUp(KeyCode.LeftShift))
            {
                speed = walkspeed;
            }

            //Move Person
            if(fpsCam.enabled)GetInputs();
            if (Input.GetButtonDown("Jump") && isGrounded && Time.timeScale > 0)
            {
                //jump
                rbody.velocity += (Vector3.up * jumpheight);
                //GetComponent<Rigidbody>().AddForce(transform.up * jumpheight);
            }
            setSelectedPersonText();
        }
        
    }

    private void switchToTabletop()
    {
        fpsCam.transform.rotation = Quaternion.Euler(0, fpsCam.transform.rotation.eulerAngles.y, 0);//keep the person facing up

        //deltaPosition = ((transform.forward * 0) + (transform.right * 0));
        speed = horizontal = vertical = mouseX = mouseY = 0f; //make person stop
        if(anim != null) anim.SetBool("isRunning", false);  //make person stop
        fpsCam.enabled = false;
        isControlled = false;
        GetComponentInChildren<AudioListener>().enabled = false;
        GetComponent<characterMovement>().controlManually(false);   //turn the ai back on
        GetComponent<personInteraction>().setControlled(false);
        tabletopCam.enabled = true;
        speed = walkspeed;
        GetComponent<BoxCollider>().enabled = false;
        GetComponent<personInteraction>().resetNearObjectsList();
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        tabletopCam.gameObject.GetComponent<cameraControl>().returnToTabletop(transform.position);
    }

    private void changeTerrain(RaycastHit hit, float strength)
    {
        //change terrain
        object[] tempStorage = new object[3];
        tempStorage[0] = hit.point;
        tempStorage[1] = .2f * strength;
        tempStorage[2] = 1;
        hit.collider.gameObject.SendMessage("changeTerrain", tempStorage);
    }

    private void hitNatureObject(RaycastHit hit)
    {
        hit.collider.gameObject.SendMessageUpwards("hitObject",GetComponent<personAttributes>().type);
    }

    private void FixedUpdate()
    {
        if (fpsCam.enabled)
        {
            deltaPosition = ((transform.forward * vertical) + (transform.right * horizontal)) * speed * Time.fixedDeltaTime;
            rbody.MovePosition(rbody.position + deltaPosition);
        }
    }

    public void eat()
    {
        if (!gameStatistic.GS.eatFood(1))
        {
            decreaseHealth(20);
        }
        else
        {
            if(gameStatistic.GS.beds >= gameStatistic.GS.population)
            {
                increaseHealth(10);
            }
        }
    }

    public void eatFood()
    {
        if (gameStatistic.GS.eatFood(5))
        {
            increaseHealth(10);
        }
    }

    public void repairArmor()
    {
        if (gameStatistic.GS.takeIron(1))
        {
            increaseArmor(5);
        }
    }

    void GetInputs()
    {
        mouseX = Input.GetAxis("Mouse X");
        mouseY = Input.GetAxis("Mouse Y");
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        if(anim != null)
        {
            if (horizontal != 0 || vertical != 0)
            {
                anim.SetBool("isRunning", true);
            }
            else
            {
                anim.SetBool("isRunning", false);
            }
            //Debug.Log("hor: " + horizontal + " vert: " + vertical);
        }
    }

    public void controlPerson()
    {
        isControlled = true;
        //GetComponent<personInteraction>().resetNearObjectsList();
        GetComponent<personInteraction>().setControlled(true);
        GetComponent<BoxCollider>().enabled = true;
        RenderSettings.fogDensity = Mathf.InverseLerp(0, 2500, 20);
    }

    public void applyDamage(int amount)
    {
        GetComponent<personAttributes>().armor -= amount;

        if (fpsCam.enabled)     
        {
            BloodBorderAnim.SetTrigger("hit");
        }

        if (GetComponent<personAttributes>().armor < 0)
        {
            GetComponent<personAttributes>().health += GetComponent<personAttributes>().armor;
            GetComponent<personAttributes>().armor = 0;
            if (GetComponent<personAttributes>().health <= 0)
            {
                if (fpsCam.enabled) switchToTabletop();

                anim.SetTrigger("isDead");
                GetComponent<personAttributes>().health = 0;
                GetComponent<characterMovement>().speed = 0;
                transform.GetComponent<Rigidbody>().useGravity = false;
                transform.GetChild(0).GetComponent<CapsuleCollider>().enabled = false;                
                destroyPerson();
            }
        }
    }

    private void decreaseHealth(int amount)
    {
        GetComponent<personAttributes>().health -= amount;        

        if (GetComponent<personAttributes>().health <= 0)
        {
            if (fpsCam.enabled) switchToTabletop();

            anim.SetTrigger("isDead");
            GetComponent<personAttributes>().health = 0;
            GetComponent<characterMovement>().speed = 0;
            transform.GetComponent<Rigidbody>().useGravity = false;
            transform.GetChild(0).GetComponent<CapsuleCollider>().enabled = false;
            destroyPerson();
        }
    }

    private void increaseHealth(int amount)
    {
        GetComponent<personAttributes>().health += amount;
        if (GetComponent<personAttributes>().health > GetComponent<personAttributes>().maxhealth) GetComponent<personAttributes>().health = GetComponent<personAttributes>().maxhealth;
    }

    private void increaseArmor(int amount)
    {
        GetComponent<personAttributes>().armor += amount;
        if (GetComponent<personAttributes>().armor > GetComponent<personAttributes>().maxarmor) GetComponent<personAttributes>().armor = GetComponent<personAttributes>().maxarmor;
    }

    public void hitObject(int type)
    {
        decreaseHealth(5);
    }

    private void setSelectedPersonText()
    {
        txtPersonHealth.text = GetComponent<personAttributes>().health.ToString() + " / " + GetComponent<personAttributes>().maxhealth.ToString();
        healthBar.fillAmount = GetComponent<personAttributes>().health / GetComponent<personAttributes>().maxhealth;
        txtPersonArmor.text = GetComponent<personAttributes>().armor.ToString() + " / " + GetComponent<personAttributes>().maxarmor.ToString();
        armorBar.fillAmount = GetComponent<personAttributes>().armor / GetComponent<personAttributes>().maxarmor;
        txtPersonJob.text = GetComponent<personAttributes>().getJobName();
    }

    void destroyPerson()
    {
        isDead = true;
        gameStatistic.GS.persons.Remove(gameObject);
        Destroy(gameObject, 4f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(!collision.gameObject.CompareTag("person") && !collision.gameObject.CompareTag("military"))
        {
            isGrounded = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (!collision.gameObject.CompareTag("person") && !collision.gameObject.CompareTag("military"))
        {
            isGrounded = false;
        }
    }
}
