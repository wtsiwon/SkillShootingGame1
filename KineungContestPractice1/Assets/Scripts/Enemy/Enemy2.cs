using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy2 : Enemy
{
    protected override void Attack()
    {
        CircleDelayShot(12);
    }

    private void CircleDelayShot(int count)
    {
        StartCoroutine(IShot(count));
    }

    private IEnumerator IShot(int count)
    {
        for (int i = 0; i < 360; i+= 360 / count)
        {
            Quaternion rot = Quaternion.Euler(0, 0, i);
            Bullet bullet1 = ObjPool.Instance.GetBullet(transform.position, 4, rot);

            bullet1.SetBullet(transform.position, rot, bulletSpd, dmg, true,4);
        }
        yield break;
    }
}
