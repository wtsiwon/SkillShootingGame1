using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrikerPet : Pet
{
    protected override void Attack()
    {

    }

    private void Skrike()
    {
        Bullet bullet1 = Instantiate(bullet, transform.position, Quaternion.identity);
        //bullet1.SetBullet(transform.position, Vector3.up, player.bulletSpd / 2, atkDmg *= (float)player.Level += (atkDmg *= player.Level) * 0.75f )
    }
}
