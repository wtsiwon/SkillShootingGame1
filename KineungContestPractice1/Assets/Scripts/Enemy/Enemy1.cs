using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1 : Enemy
{
    protected override void Attack()
    {

        StartCoroutine(SectorFormShot(3));
    }

    private IEnumerator SectorFormShot(int count)
    {
        float[] amounts = new float[3] { 150, 180, 210 };

        for (int i = 0; i < count; i++)
        {
            Quaternion rot = Quaternion.Euler(0, 0, amounts[i]);

            Bullet bullet1 = ObjPool.Instance.GetBullet(transform.position, 4, rot);
            bullet1.SetBullet(transform.position, rot, bulletSpd, dmg, true, 4);

            yield return null;
        }
    }
    
    

}
