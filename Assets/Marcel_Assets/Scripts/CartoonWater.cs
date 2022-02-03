using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CartoonWater : MonoBehaviour
{
    public float scrollSpeedX = 0.05f;
    public float scrollSpeedY = 0.05f;
    Renderer rend;

    public float height;
    public float time;

    void Start()
    {
        rend = GetComponent<Renderer>();

        //iTween.MoveBy(this.gameObject, iTween.Hash("y", height, "time", time, "looptype", "pingpong", "easetype", iTween.EaseType.easeInOutSine));
    }

    void Update()
    {
        float offsetX = Time.time * scrollSpeedX;
        float offsetY = Time.time * scrollSpeedY;
        rend.material.SetTextureOffset("_MainTex", new Vector2(offsetX, offsetY));
    }
}
