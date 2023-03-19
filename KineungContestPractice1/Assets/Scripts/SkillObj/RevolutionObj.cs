using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RevolutionObj : MonoBehaviour
{
    [HideInInspector]
    public float rotateSpd;

    private void Update()
    {
        transform.Rotate(0, 0, rotateSpd * Time.deltaTime);
    }
}
