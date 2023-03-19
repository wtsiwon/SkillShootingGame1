using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RevolutionTriggerObj : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Enemy enemy))
        {
            float dmg = 0f;
            if (enemy.isBoss == false)
            {
                dmg = enemy.maxHp / (100 / 25);
                //�ִ� ü���� 25%
            }
            else
            {
                dmg = enemy.maxHp / (100 / 0.5f);
                //�ִ� ü���� 0.5%
            }

            print(dmg);
            enemy.OnDamaged(dmg);
            GameManager.Instance.GetDestroyEffect(transform.position, 3.5f);
        }
    }
}
