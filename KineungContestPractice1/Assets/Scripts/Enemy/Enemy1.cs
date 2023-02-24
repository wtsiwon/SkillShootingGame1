using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1 : Enemy
{
    protected override void Attack()
    {
        StartCoroutine(SectorFormShot(5, transform.position.z));
    }

    private IEnumerator SectorFormShot(int count, float central)
    {
        float amount = central / (count - 1);
        float z = central / -2f;

        for (int i = 0; i < count; i++)
        {
            Vector3 rot = new Vector3(0, 0, z);
            Bullet bullet1 = Instantiate(bullet);
            bullet.SetBullet(transform.position, rot, bulletSpd, 1, 1, true);
            z += amount;
        }
        yield return null;
    }
}
