using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundScroll : MonoBehaviour
{
    public float scrollSpd;
    void Update()
    {
        transform.position += Vector3.down * scrollSpd * Time.deltaTime;

        if(transform.position.y < -13)
        {
            transform.position = new Vector3(0, 15, 0);
        }
    }
}