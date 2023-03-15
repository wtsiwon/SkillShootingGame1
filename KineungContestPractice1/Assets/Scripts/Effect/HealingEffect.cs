using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingEffect : MonoBehaviour
{
    public float moveSpd;
    private void Update()
    {
        transform.Translate(Vector3.up * moveSpd * Time.deltaTime);
    }
}
