using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicPet : Pet
{


    protected override void Update()
    {
        base.Update();
    }
    protected override void Attack()
    {
        BasicAttack();
    }

    private void BasicAttack()
    {
        Bullet bullet1 = Instantiate(bullet, transform.position, Quaternion.identity);

        Quaternion rot = Quaternion.Euler(Vector3.up);

        bullet1.SetBullet(transform.position, rot, player.bulletSpd, atkDmg * player.Level, false);
    }
    

}
