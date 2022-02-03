using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/*
 * By Nicolas Schoen
 * 
 * Camera Controller Script
*/

public class cameraControl : MonoBehaviour
{
    [Header("Camera Adjustments")]
    private float speed = 2.0f;
    public float normalSpeed = .05f;
    public float rotationSpeed = 45f;
    public float scrollfactor = 5.0f;
    public float shiftspeed = 2.0f;
    public float maxheight = 6.0f;
    public float minheight = 1.0f;
    public float raylength = 100f;
    private float mouseEdgeMovement = 5f;

    public float maxWidth = 100;
    public float originX = 0;
    public float originZ = 0;

    public LayerMask layermask;
    public Camera tabletopCam;
    public GameObject[] placableBuilding;
    public int selectedObject = 0;
    private int selectedWidth = 0;
    private int selectedLength = 0;
    private int selectedRotation = 0;
    private GameObject currentSelectedBuilding;
    public GameObject clickObject;

    public GameObject placementIndicator;
    public Material placementGood;
    public Material placementBad;
    public Material placementNoResources;

    private bool objectSelected = false;
    public List<GameObject> selectedGameObject = new List<GameObject>();    //objects selected within tabletop view
    private bool mouseOverMenu = false;
    private bool isRemovingMode = false;
    private bool isJobMode = false;

    public AudioSource clickSound;
    public AudioSource placeBuildingSound;
    public AudioSource destroyBuildingSound;

    [Header("UI Elements")]
    public Canvas generalCanvas;
    public Canvas fpsCanvas;
    public Canvas buildCanvas;
    public Canvas pauseCanvas;
    public Canvas recruitCanvas;
    public Canvas recruitMilitaryCanvas;
    public Canvas recruitMageCanvas;
    public Canvas saveCanvas;
    public Canvas personInfoCanvas;
    public GameObject recruitShooterButton;
    public GameObject recruitMeleeButton;
    public GameObject recruitMinerButton;
    public GameObject recruitBuilderButton;
    public GameObject recruitWoodcutterButton;
    public GameObject recruitFarmerButton;
    public GameObject recruitScoutButton;
    public GameObject recruitKnightButton;
    public GameObject recruitMageButton;
    public TextMeshProUGUI txtBuildingName;
    public TextMeshProUGUI txtBuildingDescription;
    public TextMeshProUGUI txtBuildingWood;
    public TextMeshProUGUI txtBuildingStone;
    public TextMeshProUGUI txtBuildingIron;
    public TextMeshProUGUI txtBuildingCount;
    public TextMeshProUGUI txtPersonHealth;
    public TextMeshProUGUI txtPersonArmor;
    public TextMeshProUGUI txtPersonJob;
    public Image healthBar;
    public Image armorBar;
    public GameObject removingModePanel;
    public GameObject jobModePanel;
    private Color redColor;
    private Color greenColor;

    public TMP_InputField filename;

    bool isDragging = false;
    Vector3 mousePosition;

    const int fogDensity = 4000;

    // Start is called before the first frame update
    void Start()
    {
        speed = normalSpeed;
        //Instantiate(placementIndicator, new Vector3(50, 8, 50), Quaternion.identity);
        placementIndicator.SetActive(false);
        selectedLength = placableBuilding[selectedObject].GetComponent<ModelAttributes>().getLength();
        selectedWidth = placableBuilding[selectedObject].GetComponent<ModelAttributes>().getWidth();
        redColor = recruitShooterButton.GetComponent<Image>().color;
        greenColor = recruitMinerButton.GetComponent<Image>().color;

        transform.parent.position = new Vector3(globalVariables.mapSize/2, MapController.MC.getHeight(globalVariables.mapSize / 2, globalVariables.mapSize / 2), globalVariables.mapSize / 2);
        RenderSettings.fogDensity = Mathf.InverseLerp(0, fogDensity, tabletopCam.fieldOfView);
        if (!globalVariables.loadSaveGame)GetComponent<populationManagement>().spawnStartPeople();        
    }

    private void OnGUI()        //Draw white Rectangle
    {
        if (isDragging)
        {
            //selectedCharacters = 0;
            var rect = ScreenHelper.GetScreenRect(mousePosition, Input.mousePosition);
            ScreenHelper.DrawScreenRect(rect, new Color(0.85f, 0.85f, 0.95f, 0.1f));
            ScreenHelper.DrawScreenRectBorder(rect, 2, Color.white);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (tabletopCam.enabled)
        {
            if (buildCanvas.enabled && !pauseCanvas.enabled)
            {
                handleBuildMenu();
            }
            else if(recruitCanvas.enabled && !pauseCanvas.enabled)
            {
                handleRecruitMenu();
            }
            else if (recruitMilitaryCanvas.enabled && !pauseCanvas.enabled)
            {
                handleRecruitMilitaryMenu();
            }
            else if (recruitMageCanvas.enabled && !pauseCanvas.enabled)
            {
                handleRecruitMageMenu();
            }
            else if (!pauseCanvas.enabled)
            {
                handleNormalGameplay();
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                togglePause();
            }

            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                speed = normalSpeed * shiftspeed;
                rotationSpeed = rotationSpeed * shiftspeed;
            }
            if (Input.GetKeyUp(KeyCode.LeftShift))
            {
                speed = normalSpeed;
                rotationSpeed = 45f;
            }

            //toggle build canvas
            if (!pauseCanvas.enabled)
            {
                checkCameraMovement();
            }
        }

    }

    private void handleBuildMenu()
    {
        if (!mouseOverMenu)
        {
            int localselectedLength;
            int localselectedWidth;
            if (selectedRotation == 90 || selectedRotation == 270)
            {
                localselectedLength = selectedWidth;
                localselectedWidth = selectedLength;
            }
            else
            {
                localselectedLength = selectedLength;
                localselectedWidth = selectedWidth;
            }
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, raylength, layermask))
            {
                //Debug.Log(hit.collider.name);
                if (hit.collider.tag == "ground")
                {
                    Vector3 placePosition = new Vector3((int)hit.point.x, hit.point.y, (int)(hit.point.z));//rounding position to fit into blocks
                    if (Input.GetMouseButtonDown(0))    //left mouse
                    {
                        //place Building
                        object[] tempStorage = new object[6];
                        tempStorage[0] = placePosition;
                        tempStorage[1] = localselectedLength;
                        tempStorage[2] = localselectedWidth;
                        tempStorage[3] = placableBuilding[selectedObject].GetComponent<ModelAttributes>().getModelType();
                        //tempStorage[1] = 2;//place tower with length 2
                        //tempStorage[2] = 2;//place tower with width 2
                        //tempStorage[3] = 10;//value is 10(tower)
                        tempStorage[4] = placableBuilding[selectedObject];
                        tempStorage[5] = Quaternion.Euler(0, selectedRotation, 0);
                        hit.collider.gameObject.SendMessage("placeObject", tempStorage);
                        placeBuildingSound.Play();
                    }

                    if (Input.GetMouseButtonDown(1))    //right mouse
                    {
                        //change terrain
                        object[] tempStorage = new object[3];
                        tempStorage[0] = hit.point;
                        tempStorage[1] = localselectedLength;
                        tempStorage[2] = localselectedWidth;
                        hit.collider.gameObject.GetComponent<MapController>().generateBuildPlace(tempStorage);
                    }

                    if (MapController.MC.checkMap(hit.point, localselectedWidth, localselectedLength))
                    {
                        if (gameStatistic.GS.buildPossible(placableBuilding[selectedObject].GetComponent<ModelAttributes>().requiredWood, placableBuilding[selectedObject].GetComponent<ModelAttributes>().requiredStone, placableBuilding[selectedObject].GetComponent<ModelAttributes>().requiredIron))
                        {
                            //change material to good
                            placementIndicator.GetComponentInChildren<Renderer>().material = placementGood;
                        }
                        else
                        {
                            //change material to no resources
                            placementIndicator.GetComponentInChildren<Renderer>().material = placementNoResources;
                        }
                        
                    }
                    else
                    {
                        //change material to bad
                        placementIndicator.GetComponentInChildren<Renderer>().material = placementBad;
                    }
                    placementIndicator.transform.position = new Vector3(placePosition.x+((float)localselectedWidth / 2), placePosition.y, placePosition.z + ((float)localselectedLength / 2));
                    placementIndicator.transform.rotation = Quaternion.Euler(0, selectedRotation, 0);
                    placementIndicator.transform.localScale = new Vector3(selectedWidth, 1, selectedLength);

                    currentSelectedBuilding.transform.position = new Vector3(placePosition.x + ((float)localselectedWidth / 2), placePosition.y, placePosition.z + ((float)localselectedLength / 2));
                    currentSelectedBuilding.transform.rotation = Quaternion.Euler(0, selectedRotation, 0);
                }
            }
        }

        if (Input.GetKeyDown("b")) toggleBuildCanvas();


        if (Input.GetKeyDown("e")) rotatePlacobjectRight();

        if (Input.GetKeyDown("q")) rotatePlacobjectLeft();
    }

    private void rotatePlacobjectRight()
    {
        selectedRotation += 90;
        selectedRotation %= 360;
    }

    private void rotatePlacobjectLeft()
    {
        selectedRotation -= 90;
        if (selectedRotation < 0) selectedRotation = 270;
    }

    private void handleRecruitMenu()
    {
        if (recruitCanvas.enabled)
        {
            if (gameStatistic.GS.getFood() >= 40 && gameStatistic.GS.beds > gameStatistic.GS.population)
            {
                recruitBuilderButton.GetComponent<Image>().color = greenColor;
                recruitWoodcutterButton.GetComponent<Image>().color = greenColor;
                recruitMinerButton.GetComponent<Image>().color = greenColor;
                recruitFarmerButton.GetComponent<Image>().color = greenColor;
            }
            else
            {
                recruitBuilderButton.GetComponent<Image>().color = redColor;
                recruitWoodcutterButton.GetComponent<Image>().color = redColor;
                recruitMinerButton.GetComponent<Image>().color = redColor;
                recruitFarmerButton.GetComponent<Image>().color = redColor;
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                toggleRecruitCanvas();
            }
        }
    }

    private void handleRecruitMilitaryMenu()
    {
        if (gameStatistic.GS.getFood() >= 50 && gameStatistic.GS.beds > gameStatistic.GS.population)
        {
            recruitScoutButton.GetComponent<Image>().color = greenColor;
            if (gameStatistic.GS.getIron() >= 2)
            {
                recruitShooterButton.GetComponent<Image>().color = greenColor;
                if (gameStatistic.GS.getFood() >= 80 && gameStatistic.GS.getIron() >= 10)
                {
                    recruitMeleeButton.GetComponent<Image>().color = greenColor;
                    if (gameStatistic.GS.getFood() >= 100 && gameStatistic.GS.getIron() >= 20)
                    {
                        recruitKnightButton.GetComponent<Image>().color = greenColor;
                    }
                    else
                    {
                        recruitKnightButton.GetComponent<Image>().color = redColor;
                    }
                }
                else
                {
                    recruitMeleeButton.GetComponent<Image>().color = redColor;
                    recruitKnightButton.GetComponent<Image>().color = redColor;
                }
            }
            else
            {
                recruitShooterButton.GetComponent<Image>().color = redColor;
                recruitMeleeButton.GetComponent<Image>().color = redColor;
                recruitKnightButton.GetComponent<Image>().color = redColor;
            }
        }
        else
        {
            recruitScoutButton.GetComponent<Image>().color = redColor;
            recruitShooterButton.GetComponent<Image>().color = redColor;
            recruitMeleeButton.GetComponent<Image>().color = redColor;
            recruitKnightButton.GetComponent<Image>().color = redColor;
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            toggleRecruitCanvas();
        }
    }

    private void handleRecruitMageMenu()
    {
        if (recruitMageCanvas.enabled)
        {
            if (gameStatistic.GS.getFood() >= 200 && gameStatistic.GS.getIron() >= 100 && gameStatistic.GS.beds > gameStatistic.GS.population)
            {
                recruitMageButton.GetComponent<Image>().color = greenColor;
            }
            else
            {
                recruitMageButton.GetComponent<Image>().color = redColor;
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                toggleRecruitCanvas();
            }
        }
    }

    private void handleNormalGameplay()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, raylength, layermask))
            {
                Debug.Log(hit.collider.name);
                if (hit.collider.tag != "person" && hit.collider.tag != "military")
                {
                    mousePosition = Input.mousePosition;
                    isDragging = true;
                    deSelectPerson();
                }
            }
        }
        if (Input.GetMouseButtonUp(0))    //left mouse
        {
            if (isDragging)
            {
                selectedGameObject.Clear();
                foreach (var selectableObject in FindObjectsOfType<personAttributes>())
                {
                    if (IsWithinSelectionBounds(selectableObject.transform))
                    {
                        //selectedCharacters++;
                        //selectableObject.selectPerson();
                        if (!selectedGameObject.Contains(selectableObject.gameObject)) selectedGameObject.Add(selectableObject.gameObject);
                        //selectedGameObject[selectedCharacters] = selectableObject.gameObject;
                        Debug.Log("Character multiple: " + selectedGameObject.Count);
                        if (!buildCanvas.enabled && !pauseCanvas.enabled)
                        {
                            if (selectableObject.gameObject.GetComponent<characterMovement>().selectPerson())
                            {
                                //deSelectPerson();

                                if (!selectedGameObject.Contains(selectableObject.gameObject)) selectedGameObject.Add(selectableObject.gameObject);
                            }
                            else
                            {
                                selectedGameObject[selectedGameObject.IndexOf(selectableObject.gameObject)].GetComponent<characterMovement>().selectPerson();
                                deSelectPerson(selectableObject.gameObject);
                                //selectedGameObject = null;
                            }
                            Debug.Log(selectedGameObject.Count);
                        }
                    }
                }
                //Debug.Log("Selected Characters: " + selectedCharacters);
                isDragging = false;
            }
            else
            {
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit, raylength, layermask))
                {
                    Debug.Log(hit.collider.name);
                    if (hit.collider.tag == "person" || hit.collider.tag == "military")
                    {
                        if (!buildCanvas.enabled && !pauseCanvas.enabled)
                        {
                            if (hit.collider.gameObject.GetComponentInParent<characterMovement>().selectPerson())
                            {
                                //deSelectPerson();
                                if (selectedGameObject.Count >= 1) deSelectPerson();
                                if (!selectedGameObject.Contains(hit.collider.transform.parent.gameObject)) selectedGameObject.Add(hit.collider.transform.parent.gameObject);
                            }
                            else
                            {
                                selectedGameObject[selectedGameObject.IndexOf(hit.collider.transform.parent.gameObject)].GetComponent<characterMovement>().selectPerson();
                                deSelectPerson(hit.collider.transform.parent.gameObject);
                                //selectedGameObject = null;
                            }
                            Debug.Log(selectedGameObject.Count);
                        }
                    }

                    if (hit.collider.tag == "ground")    //deselect Person when left clicking on ground
                    {
                        deSelectPerson();
                    }
                }
            }
        }
        if (Input.GetMouseButtonDown(1) && !pauseCanvas.enabled && !mouseOverMenu)    //right mouse
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, raylength, layermask))
            {
                Debug.Log(hit.collider.name);

                if (hit.collider.tag == "ground")
                {
                    if (selectedGameObject.Count > 0)
                    {
                        foreach (GameObject selectedObjct in selectedGameObject)
                        {
                            woodcutterController wc = selectedObjct.GetComponentInParent<woodcutterController>();
                            minerController mc = selectedObjct.GetComponentInParent<minerController>();
                            if (selectedObjct.tag == "military")
                            {
                                selectedObjct.SendMessage("removeEnemy");
                            }

                            if (wc != null) wc.DropJob();
                            if (mc != null) mc.DropJob();

                            selectedObjct.GetComponent<characterMovement>().setGoal(hit.point);
                        }
                        

                        GameObject Click = Instantiate(clickObject, hit.point, Quaternion.identity) as GameObject;      //Klickeffekt für ausgewählte Personen in Tabletop View
                    }
                }
                if(hit.collider.tag == "enemy")
                {
                    foreach (GameObject person in selectedGameObject)
                    {
                        if(person.tag == "military")
                        {
                            person.SendMessage("setEnemy",hit.collider.gameObject);
                        }
                    }
                }
                if (isRemovingMode)
                {
                    if(hit.collider.tag == "ownMilitaryBuilding")
                    {
                        hit.collider.gameObject.SendMessageUpwards("destroyBuilding");
                        destroyBuildingSound.Play();
                    }
                    if (hit.collider.tag == "building_civilian")
                    {
                        hit.collider.gameObject.SendMessageUpwards("destroyBuilding");
                        destroyBuildingSound.Play();
                    }
                    if (hit.collider.tag == "building_military")
                    {
                        hit.collider.gameObject.SendMessageUpwards("destroyBuilding");
                        destroyBuildingSound.Play();
                    }
                    if (hit.collider.tag == "mageTower")
                    {
                        hit.collider.gameObject.SendMessageUpwards("destroyBuilding");
                        destroyBuildingSound.Play();
                    }
                }
                else if (isJobMode)
                {
                    if (hit.collider.tag == "nature")
                    {
                        hit.collider.gameObject.SendMessageUpwards("clickedOnObject");
                        destroyBuildingSound.Play();
                    }
                }
                else
                {
                    if (hit.collider.tag == "ownMilitaryBuilding")
                    {
                        if (hit.collider.transform.root.GetComponent<AudioSource>() != null) hit.collider.transform.root.GetComponent<AudioSource>().Play();
                        toggleRecruitCanvas(true,false);
                    }
                    if (hit.collider.tag == "mageTower")
                    {
                        if (hit.collider.transform.root.GetComponent<AudioSource>() != null) hit.collider.transform.root.GetComponent<AudioSource>().Play();
                        toggleRecruitCanvas(false, true);
                    }
                }

                
            }
        }

        if (Input.GetKeyDown("b"))
        {
            toggleBuildCanvas();
            isRemovingMode = false;
            isJobMode = false;
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            isJobMode = false;
            isRemovingMode = !isRemovingMode;
        }

        if (Input.GetKeyDown(KeyCode.Y))
        {
            isRemovingMode = false;
            isJobMode = !isJobMode;
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            toggleRecruitCanvas();
            isRemovingMode = false;
            isJobMode = false;
        }

        if (Input.GetKeyDown(KeyCode.F1)) togglePostcardMode();

        if (Input.GetKeyDown(KeyCode.C))    //switch to selected person
        {
            if (selectedGameObject.Count == 1)
            {
                if(selectedGameObject[0] != null)
                {
                    //selectedGameObject.GetComponent<characterMovement>().selectPerson(); //deselect person when switching to person
                    selectedGameObject[0].GetComponent<characterMovement>().controlManually(true); //disable the ai control
                    selectedGameObject[0].GetComponentInChildren<Camera>().enabled = true;
                    selectedGameObject[0].GetComponentInChildren<AudioListener>().enabled = true;
                    tabletopCam.enabled = false;
                    GetComponent<AudioListener>().enabled = false;
                    //GetComponentInParent<AudioListener>().enabled = false;
                    Cursor.visible = false;
                    Cursor.lockState = CursorLockMode.Locked;
                    fpsCanvas.enabled = true;
                    selectedGameObject[0].GetComponent<personController>().controlPerson();
                    //selectedGameObject = null;

                    woodcutterController wc = selectedGameObject[0].GetComponentInParent<woodcutterController>();
                    minerController mc = selectedGameObject[0].GetComponentInParent<minerController>();

                    if (wc != null) wc.DropJob();
                    if (mc != null) mc.DropJob();
                    deSelectPerson();
                }
                else
                {
                    selectedGameObject.Clear();
                }
                
            }
        }

        if (Input.GetKey("e"))
        {
            rotateCameraRight();
        }

        if (Input.GetKey("q"))
        {
            rotateCameraLeft();
        }

        removingModePanel.SetActive(isRemovingMode);
        jobModePanel.SetActive(isJobMode);


        setSelectedPersonText();
    }

    private bool IsWithinSelectionBounds(Transform transform)
    {
        if (!isDragging)
        {
            return false;
        }

        var viewportBounds = ScreenHelper.GetViewportBounds(tabletopCam, mousePosition, Input.mousePosition);
        return viewportBounds.Contains(tabletopCam.WorldToViewportPoint(transform.position));
    }

    private void deSelectPerson(GameObject thatPerson = null)
    {
        if(thatPerson != null)
        {
            selectedGameObject[selectedGameObject.IndexOf(thatPerson)].GetComponent<characterMovement>().selectPerson(); //deselect person when switching to person
            selectedGameObject.Remove(thatPerson);
            return;
        }
        if (selectedGameObject.Count > 0)
        {
            foreach (GameObject curntGameobjct in selectedGameObject)
            {
                curntGameobjct.GetComponent<characterMovement>().selectPerson(); //deselect person when switching to person
            }
            
            selectedGameObject.Clear();
            
        }
    }

    private void setSelectedPersonText()
    {
        if (selectedGameObject.Count == 1)
        {
            if (selectedGameObject[0] != null)
            {
                personInfoCanvas.enabled = true;
                txtPersonHealth.text = selectedGameObject[0].GetComponent<personAttributes>().health.ToString() + " / " + selectedGameObject[0].GetComponent<personAttributes>().maxhealth.ToString();
                healthBar.fillAmount = selectedGameObject[0].GetComponent<personAttributes>().health / selectedGameObject[0].GetComponent<personAttributes>().maxhealth;
                txtPersonArmor.text = selectedGameObject[0].GetComponent<personAttributes>().armor.ToString() + " / " + selectedGameObject[0].GetComponent<personAttributes>().maxarmor.ToString();
                armorBar.fillAmount = selectedGameObject[0].GetComponent<personAttributes>().armor / selectedGameObject[0].GetComponent<personAttributes>().maxarmor;
                txtPersonJob.text = selectedGameObject[0].GetComponent<personAttributes>().getJobName();
                return;
            }
            else
            {
                selectedGameObject.Clear();
            }

        }

        personInfoCanvas.enabled = false;
    }

    private void rotateCameraLeft()
    {
        transform.parent.Rotate(0,-rotationSpeed*Time.unscaledDeltaTime,0);
    }

    private void rotateCameraRight()
    {
        transform.parent.Rotate(0, rotationSpeed*Time.unscaledDeltaTime, 0);
    }

    private void checkCameraMovement()
    {
        if (Input.GetKey("up") || Input.GetKey("w") || Input.mousePosition.y >= Screen.height - mouseEdgeMovement)
        {
            //if (transform.position.z < maxWidth) 
            checkCameraPosition(transform.parent.position, transform.parent.forward * speed * (transform.position.y / 2) * tabletopCam.fieldOfView * Time.unscaledDeltaTime);
        }
        if (Input.GetKey("down") || Input.GetKey("s") || Input.mousePosition.y <= mouseEdgeMovement)
        {
            //if (transform.position.z > originZ) 
            checkCameraPosition(transform.parent.position , transform.parent.forward * -1 * speed * (transform.position.y / 2) * tabletopCam.fieldOfView * Time.unscaledDeltaTime);
        }
        if (Input.GetKey("left") || Input.GetKey("a") || Input.mousePosition.x <= mouseEdgeMovement)
        {
            //if (transform.position.x > -originX) 
            checkCameraPosition(transform.parent.position , transform.parent.right * -1 * speed * (transform.position.y / 2) * tabletopCam.fieldOfView * Time.unscaledDeltaTime);
        }
        if (Input.GetKey("right") || Input.GetKey("d") || Input.mousePosition.x >= Screen.width - mouseEdgeMovement)
        {
            //if (transform.position.x < maxWidth) 
            checkCameraPosition(transform.parent.position , transform.parent.right * speed * (transform.position.y / 2) * tabletopCam.fieldOfView * Time.unscaledDeltaTime);
        }
        if (Input.mouseScrollDelta.y < 0)
        {
            //alte camera position: 0,2,-5
            //if (tabletopCam.fieldOfView < 70) tabletopCam.fieldOfView += scrollfactor * tabletopCam.fieldOfView * speed;
            RenderSettings.fogDensity = Mathf.InverseLerp(0, fogDensity, tabletopCam.fieldOfView);
            if (transform.position.y < maxheight) transform.position += (Vector3.up - transform.forward.normalized*2) * speed * scrollfactor * Time.unscaledDeltaTime;
        }
        if (Input.mouseScrollDelta.y > 0)
        {
            //if (tabletopCam.fieldOfView > 5) tabletopCam.fieldOfView -= scrollfactor * tabletopCam.fieldOfView * speed;
            RenderSettings.fogDensity = Mathf.InverseLerp(0, fogDensity, tabletopCam.fieldOfView);
            if (transform.position.y > minheight) transform.position += (-Vector3.up + transform.forward.normalized*2) * speed * scrollfactor * Time.unscaledDeltaTime;
        }
    }

    private void checkCameraPosition(Vector3 oldPosition, Vector3 newPosition)
    {
        newPosition = oldPosition + newPosition;
        if (newPosition.x > originX && newPosition.z > originZ && newPosition.x <= globalVariables.mapSize && newPosition.z <= globalVariables.mapSize)
        {
            transform.parent.position = newPosition;
        }
        else
        {
            transform.parent.position = oldPosition;
        }        
    }

    public void toggleBuildCanvas()
    {
        deSelectPerson();
        clickSound.Play();
        isDragging = false;
        buildCanvas.enabled = !buildCanvas.enabled;
        if (buildCanvas.enabled)
        {
            currentSelectedBuilding = Instantiate(placableBuilding[selectedObject],Vector3.zero,Quaternion.identity);
            setLayerRecursive(currentSelectedBuilding, 2);

            placementIndicator.SetActive(!mouseOverMenu);

            txtBuildingName.text = placableBuilding[selectedObject].GetComponent<ModelAttributes>().buildingName;
            txtBuildingDescription.text = placableBuilding[selectedObject].GetComponent<ModelAttributes>().description;
            txtBuildingWood.text = placableBuilding[selectedObject].GetComponent<ModelAttributes>().getRequiredWood().ToString();
            txtBuildingStone.text = placableBuilding[selectedObject].GetComponent<ModelAttributes>().getRequiredStone().ToString();
            txtBuildingIron.text = placableBuilding[selectedObject].GetComponent<ModelAttributes>().getRequiredIron().ToString();
            txtBuildingCount.text = (selectedObject + 1) + " / " + placableBuilding.Length;
        }
        else
        {
            //Debug.Log("---Disabling Placement Indicator");
            placementIndicator.SetActive(false);

            Destroy(currentSelectedBuilding);
        }
        MapController.MC.setMeshTextureGrid(buildCanvas.enabled);
    }

    private void setLayerRecursive(GameObject gobject, int layer)
    {
        gobject.layer = layer;
        foreach (Transform child in gobject.transform)
        {
            setLayerRecursive(child.gameObject, layer);
        }
    }

    public void toggleRecruitCanvas(bool isMilitary = false, bool isMageTower = false)
    {
        clickSound.Play();
        isDragging = false;
        if (isMilitary)
        {
            recruitMilitaryCanvas.enabled = true;
        }
        else if (isMageTower)
        {
            recruitMageCanvas.enabled = true;
        }
        else
        {
            recruitCanvas.enabled = !recruitCanvas.enabled;
            if (recruitMilitaryCanvas.enabled)
            {
                recruitMilitaryCanvas.enabled = false;
                recruitCanvas.enabled = false;
            }
            if (recruitMageCanvas.enabled)
            {
                recruitMageCanvas.enabled = false;
                recruitCanvas.enabled = false;
            }
        }
    }

    public void closeRecruitCanvas()
    {
        toggleRecruitCanvas();
    }

    public void changeBuilding(int value)
    {
        clickSound.Play();
        Debug.Log("change building");
        if (value > 0) selectedObject = (selectedObject + value) % placableBuilding.Length;
        else selectedObject = (selectedObject + value);

        if (selectedObject < 0) selectedObject = placableBuilding.Length - 1;

        txtBuildingName.text = placableBuilding[selectedObject].GetComponent<ModelAttributes>().buildingName;
        txtBuildingDescription.text = placableBuilding[selectedObject].GetComponent<ModelAttributes>().description;
        txtBuildingWood.text = placableBuilding[selectedObject].GetComponent<ModelAttributes>().getRequiredWood().ToString();
        txtBuildingStone.text = placableBuilding[selectedObject].GetComponent<ModelAttributes>().getRequiredStone().ToString();
        txtBuildingIron.text = placableBuilding[selectedObject].GetComponent<ModelAttributes>().getRequiredIron().ToString();
        txtBuildingCount.text = (selectedObject + 1) + " / " + placableBuilding.Length;

        selectedLength = placableBuilding[selectedObject].GetComponent<ModelAttributes>().getLength();
        selectedWidth = placableBuilding[selectedObject].GetComponent<ModelAttributes>().getWidth();


        Destroy(currentSelectedBuilding);
        currentSelectedBuilding = Instantiate(placableBuilding[selectedObject], Vector3.zero, Quaternion.identity);
        setLayerRecursive(currentSelectedBuilding, 2);
    }

    public void togglePause()
    {
        clickSound.Play();
        pauseCanvas.enabled = !pauseCanvas.enabled;
        if (pauseCanvas.enabled)
        {
            Time.timeScale = 0;
        }
        else
        {
            saveCanvas.enabled = false;
            Time.timeScale = 1;
        }
    }

    public void togglePostcardMode()
    {
        generalCanvas.enabled = !generalCanvas.enabled;
    }

    public void loadMenu()
    {
        clickSound.Play();
        Time.timeScale = 1f;
        globalVariables.loadSaveGame = false;
        SceneManager.LoadScene("menu");
    }

    public void toggleMouseOverMenu()
    {
        mouseOverMenu = !mouseOverMenu;
        //Debug.Log("Mouse Over Menu " + mouseOverMenu.ToString());
        if (buildCanvas.enabled) placementIndicator.SetActive(!mouseOverMenu);
    }

    public void returnToTabletop(Vector3 playerPos)
    {
        if (playerPos.x >= globalVariables.mapSize || playerPos.x < 0 || playerPos.z >= globalVariables.mapSize || playerPos.z < 0) playerPos = new Vector3(globalVariables.mapSize/2,10,globalVariables.mapSize/2);
        fpsCanvas.enabled = false;
        GetComponent<AudioListener>().enabled = true;
        //GetComponentInParent<AudioListener>().enabled = true;
        transform.parent.SetPositionAndRotation(new Vector3(playerPos.x, playerPos.y, playerPos.z), transform.parent.rotation);
        RenderSettings.fogDensity = Mathf.InverseLerp(0, fogDensity, tabletopCam.fieldOfView);
        //transform.position.Set(playerPos.x,transform.position.y,playerPos.z -10);   //tabletop cam now over player
    }

    public void toggleSaveCanvas()
    {
        clickSound.Play();
        saveCanvas.enabled = !saveCanvas.enabled;
        filename.text = globalVariables.loadedSaveName;
    }
}
