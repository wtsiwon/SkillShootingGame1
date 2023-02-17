using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EPetType
{
    Basic,
    Striker,
    MachineGun,
    Guided,
    Bomber,
}
public abstract class Pet : MonoBehaviour
{
    public EPetType type;

    protected float atkSpd;
    protected float dmg;
    protected int level;

    protected virtual void Start()
    {
        StartCoroutine(nameof(IAttack));
    }
    protected virtual void Update()
    {
        transform.position = Player.Instance.transform.position + Player.Instance.petPosList[(int)type];
    }

    protected virtual IEnumerator IAttack()
    {
        while (true)
        {
            yield return new WaitForSeconds(atkSpd);
            Attack();
        }
    }

    protected abstract void Attack();
}
