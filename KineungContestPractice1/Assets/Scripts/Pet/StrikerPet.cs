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
        Bullet bullet1 = Instantiate(bullet, transform.position, Quaternion.identity);
        float Dmg = atkDmg * player.Level;
        Dmg += attackPowerCoefficient;

        bullet1.SetBullet(transform.position, Vector3.up, player.bulletSpd * 2, Dmg, false);
        bullet.SetScale(14f);
    }
}
