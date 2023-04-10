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

        Bullet bullet1 = ObjPool.Instance.GetBullet(transform.position, 1, Quaternion.identity);

        Quaternion rot = Quaternion.Euler(Vector3.up);
        bullet1.SetBullet(transform.position, rot, player.bulletSpd, atkDmg / 2, false, 1);
        bullet1.transform.position += new Vector3(-shootPos.x, shootPos.y, 0);
        //bullet1.transform.localScale = new Vector3(0.5f, 0.5f, 1);
        bullet1.SetScale(4);
        yield return new WaitForSeconds(0.1f);

        Bullet bullet2 = ObjPool.Instance.GetBullet(transform.position, 1, Quaternion.identity);
        Quaternion rot1 = Quaternion.Euler(Vector3.up);
        bullet2.SetBullet(transform.position, rot, player.bulletSpd, atkDmg / 2, false, 1);
        bullet2.transform.position += new Vector3(shootPos.x, shootPos.y, 0);
        //bullet2.transform.localScale = new Vector3(0.5f, 0.5f, 1);
        bullet2.SetScale(4);
        yield return new WaitForSeconds(0.1f);
    }
}
