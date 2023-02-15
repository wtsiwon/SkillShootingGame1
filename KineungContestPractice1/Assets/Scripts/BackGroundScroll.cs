using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundScroll : MonoBehaviour
{
    public float scrollSpd;

    private RectTransform rt;
    void Start()
    {
        rt = GetComponent<RectTransform>();
    }

    void Update()
    {
        if(rt.anchoredPosition.y < -1080)
        {
            rt.anchoredPosition = new Vector3(0, 1080,0);
        }
        rt.anchoredPosition += Vector2.down * scrollSpd * Time.deltaTime;
    }
}
