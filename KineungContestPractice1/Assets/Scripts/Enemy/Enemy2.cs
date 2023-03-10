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
        StartCoroutine(nameof(IShot));
    }

    private IEnumerator IShot(int count)
    {
        for (int i = 0; i < 360; i+= 360 / count)
        {
            Bullet bullet1 = Instantiate(bullet);
            bullet1.SetBullet(transform.position, new Vector3(0, 0, i), bulletSpd, dmg, true);
        }
        yield break;
    }
}
