using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1 : Enemy
{
    protected override void Attack()
    {

        StartCoroutine(SectorFormShot(5, transform.position.z));
    }

    private IEnumerator SectorFormShot(int count, float center)
    {
        float amount = 180 / count;

        float z = 180 + amount;

        for (int i = 0; i < count; i++)
        {
            Bullet bullet1 = Instantiate(bullet);
            bullet1.SetBullet(transform.position, new Vector3(0, 0, z), bulletSpd, dmg, true);
        }


        yield return null;
    }
    
    

}
