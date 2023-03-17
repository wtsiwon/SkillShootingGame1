using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuidedPet : Pet
{
    private float startDistance = 4f;
    private float endDistance = 2f;


    protected override void Update()
    {
        base.Update();
    }

    protected override void Attack()
    {
        Enemy enemy = FindObjectOfType<Enemy>();
        if (enemy == null) return;
        GuidedShot(enemy);
    }

    private void GuidedShot(Enemy enemy)
    {
        BezierBullet bbullet = (BezierBullet)Instantiate(bullet);
        bbullet.SetBullet(transform.position, player.bulletSpd / 10, atkDmg * player.Level * 2, enemy.transform, startDistance, endDistance, false);

        bbullet.SetScale(9);
    }
}
