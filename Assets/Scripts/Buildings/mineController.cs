using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mineController : MonoBehaviour
{
    [Tooltip("Spawnrate every x Seconds")]
    public float mineRate = 10f;
    public bool doMineIron = true;
    public AudioSource miningSound;
    private float deltaTime;
    private object isPlaced;
    // Start is called before the first frame update
    void Start()
    {
        deltaTime = mineRate;
        isPlaced = (object)GetComponent<ModelAttributes>().isPlaced;
    }

    // Update is called once per frame
    void Update()
    {
        if ((bool)isPlaced)
        {
            deltaTime -= Time.deltaTime;
            if (deltaTime < 0)
            {
                mineIron();
                deltaTime = mineRate;
            }
        }
    }

    private void mineIron()
    {
        if (doMineIron) gameStatistic.GS.collectIron(1);
        else gameStatistic.GS.collectStone(1);
        miningSound.Play();
    }
}
