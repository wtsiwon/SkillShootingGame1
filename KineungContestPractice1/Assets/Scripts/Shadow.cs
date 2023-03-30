using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shadow : MonoBehaviour
{
    public GameObject chasingObj;

    void Update()
    {
        transform.position = chasingObj.transform.position + new Vector3(0,0,2);
    }
}
