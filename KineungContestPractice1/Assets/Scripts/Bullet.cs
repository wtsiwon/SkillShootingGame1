using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public bool isMove = true;

    public float moveSpd;

    public int level;

    public Vector3 dir = Vector3.up;


    private void Start()
    {

    }

    private void Update()
    {
        if (isMove == true)
        {
            transform.Translate(dir * moveSpd * Time.deltaTime);
        }
        Destroy();
    }

    public void SetBullet(Vector3 pos, Vector3 dir, float spd, float level, bool isMoving = true)
    {
        transform.position = pos;
        moveSpd = spd;
        isMove = isMoving;
    }

    private void Destroy()
    {
        float distance = Vector3.Distance(Vector3.zero, transform.position);
        if(distance > 14f)
        {
            Destroy(gameObject);
        }
    }
}
