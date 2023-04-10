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
        Quaternion rot = Quaternion.Euler(Vector3.up);
        Bullet bullet1 = ObjPool.Instance.GetBullet(transform.position, 1, rot);

        bullet1.SetBullet(transform.position, rot, player.bulletSpd, atkDmg * player.Level, false, 1);
    }
}
