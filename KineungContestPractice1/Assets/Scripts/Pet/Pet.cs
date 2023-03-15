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

    [SerializeField]
    protected float atkSpd;

    [SerializeField]
    protected float atkDmg;

    protected float chaseTime = 0.5f;

    protected Bullet bullet;

    protected Player player;

    protected virtual void Start()
    {
        SetPet();

        StartCoroutine(nameof(IAttack));
    }
    protected virtual void Update()
    {

    }

    protected void LateUpdate()
    {
        //transform.position = Vector3.Lerp(transform.position, pos, chaseTime);
    }

    protected void SetPet()
    {
        player = Player.Instance;
        transform.localPosition = player.petPosList[(int)type];

        bullet = GameManager.Instance.bullet;
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
