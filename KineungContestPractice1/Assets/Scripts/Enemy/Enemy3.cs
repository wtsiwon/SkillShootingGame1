using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy3 : Enemy
{
    protected override void Attack()
    {
        RandTargetShot(8);
    }

    private void RandTargetShot(int count)
    {
        float radius = 1f;
        for (int i = 0; i < 360; i+=360 / count)
        {
            Vector3 pos = transform.position + new Vector3((i * Mathf.Deg2Rad) * radius, Mathf.Sin(i * Mathf.Deg2Rad) * radius, 0);

            Vector2 nor = Vector2.down;
            float z = Mathf.Atan2(nor.y, nor.x) * Mathf.Rad2Deg;
            Quaternion rot = Quaternion.Euler(0, 0, i);

            Instantiate(bullet, pos, rot);
        }
    }
}
