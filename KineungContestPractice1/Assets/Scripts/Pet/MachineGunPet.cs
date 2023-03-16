using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineGunPet : Pet
{
    public Vector3 shootPos;

    protected override void Attack()
    {
        StartCoroutine(IMachineGunAttack());
    }

    private IEnumerator IMachineGunAttack()
    {
        bullet.GetComponent<SpriteRenderer>().color = Color.gray;

        Bullet bullet1 = Instantiate(bullet);
        bullet1.SetBullet(transform.position, Vector3.up, player.bulletSpd, atkDmg / 2, false);
        bullet1.transform.position += new Vector3(-shootPos.x, shootPos.y, 0);
        //bullet1.transform.localScale = new Vector3(0.5f, 0.5f, 1);
        bullet1.SetScale(4);
        yield return new WaitForSeconds(0.1f);
        Bullet bullet2 = Instantiate(bullet);
        bullet2.SetBullet(transform.position, Vector3.up, player.bulletSpd, atkDmg / 2, false);
        bullet2.transform.position += new Vector3(shootPos.x, shootPos.y, 0);
        //bullet2.transform.localScale = new Vector3(0.5f, 0.5f, 1);
        bullet2.SetScale(4);
        yield return new WaitForSeconds(0.1f);
    }
}
