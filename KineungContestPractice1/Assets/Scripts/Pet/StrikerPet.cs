using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrikerPet : Pet
{
    [Tooltip("공격력 계수")]
    public float attackPowerCoefficient;

    protected override void Update()
    {
        base.Update();
    }

    protected override void Attack()
    {
        StrikeAttack();
    }

    private void StrikeAttack()
    {
        Quaternion rot = Quaternion.Euler(Vector3.up);

        Bullet bullet1 = ObjPool.Instance.GetBullet(transform.position, 3, rot);

        float Dmg = atkDmg * player.Level;

        Dmg += attackPowerCoefficient;


        bullet1.SetBullet(transform.position, rot, player.bulletSpd * 2, Dmg, false, 3);
        bullet.SetScale(14f);
    }
}
