using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicPet : Pet
{

    protected override void Attack()
    {
        BasicAttack();
    }

    private void BasicAttack()
    {
        Bullet bullet1 = Instantiate(bullet, transform.position, Quaternion.identity);
        bullet1.SetBullet(transform.position, Vector3.up, player.bulletSpd, atkDmg * player.Level, false);
    }
    

}
