using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class farmController : MonoBehaviour
{
    public GameObject cornField;
    public GameObject wheatField;
    private int type;
    public float spawnrate = 1f;//item every x seconds
    public int spawnRadius = 5;
    private float deltatime = 0f;
    private int ownWidth;
    private int ownLength;
    private object isPlaced;//like a pointer to the original isPlaced from modelattributes
    // Start is called before the first frame update
    void Start()
    {
        type = cornField.GetComponent<ModelAttributes>().modelType;
        deltatime = spawnrate;
        ownWidth = GetComponent<ModelAttributes>().blockWidth;
        ownLength = GetComponent<ModelAttributes>().blockHeight;
        isPlaced = (object)GetComponent<ModelAttributes>().isPlaced;
    }

    // Update is called once per frame
    void Update()
    {
        if ((bool)isPlaced && deltatime <= 0)
        {
            spawnField();
            deltatime = spawnrate;
        }
        deltatime -= Time.deltaTime;
    }

    private void spawnField()
    {
        int posx = Random.Range((int)transform.position.x - spawnRadius-1, (int)transform.position.x + ownWidth + spawnRadius-1);
        int posz = Random.Range((int)transform.position.z - spawnRadius-1, (int)transform.position.z + ownLength + spawnRadius-1);
        GameObject placedField;
        if (Random.Range(0,2) >= 1)
        {
            placedField = MapController.MC.placeObject(new Vector3(posx, MapController.MC.getHeight(posx, posz), posz), 1, 1, type, cornField, Quaternion.identity);
        }
        else
        {
            placedField = MapController.MC.placeObject(new Vector3(posx, MapController.MC.getHeight(posx, posz), posz), 1, 1, type, wheatField, Quaternion.identity);
        }
        
        if (placedField != null) placedField.GetComponent<Field>().startGrow();
    }
}
