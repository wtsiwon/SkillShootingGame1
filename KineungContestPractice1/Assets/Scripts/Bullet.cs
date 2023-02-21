using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private bool isMove = true;

    private bool isGuided;
    public bool IsGuided
    {
        get => isGuided;
        set
        {
            isGuided = value;
        }
    }

    private bool isTargetChasing;

    protected bool isEnemyBullet;

    protected float moveSpd;

    protected int level = 1;

    private Vector3 dir = Vector3.up;

    protected float dmg;
    public virtual float Dmg
    {
        get
        {
            return dmg * level;
        }
    }

    private void Start()
    {
        if (isGuided == true)
        {
            StartCoroutine(nameof(IGuidShoot));
        }
    }

    private void Update()
    {
        if (isMove == true)
        {
            if (isGuided == false)
            {
                transform.Translate(Vector3.up * moveSpd * Time.deltaTime);
            }
        }
        if (isTargetChasing == true)
        {
        }
        Destroy();
    }

    public void SetBullet(Vector3 pos, Vector3 dir, float spd, int level, float dmg, bool isEnemyBullet, bool isMoving = true, bool isGuided = false)
    {
        transform.position = pos;
        moveSpd = spd;
        isMove = isMoving;
        transform.rotation = Quaternion.Euler(dir);
        this.level = level;
        this.dmg = dmg;
        this.isEnemyBullet = isEnemyBullet;
        this.isGuided = isGuided;
    }

    private void Destroy()
    {
        float distance = Vector3.Distance(Vector3.zero, transform.position);
        if (distance > 14f)
        {
            GameManager.Instance.GetDestroyEffect(transform.position);
            Destroy(gameObject);
        }
    }

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if (isEnemyBullet == true)
        {
            if (collision.TryGetComponent<Player>(out Player player))
            {
                player.OnDamaged();
                Destroy(gameObject);
            }
        }
        else
        {
            if (collision.TryGetComponent<Enemy>(out Enemy enemy))
            {
                enemy.OnDamaged(Dmg);
                GameManager.Instance.GetDestroyEffect(transform.position, 7, true);
                Destroy(gameObject);
            }
        }
    }

    private IEnumerator IGuidShoot()
    {
        Vector3 startRot = transform.rotation.eulerAngles;
        Vector3 endRot = Vector3.zero;

        float duration = 1f;
        float timer = 0;
        while (timer < duration)
        {
            transform.rotation = Quaternion.Euler(Vector3.Lerp(startRot, endRot, timer / duration));

            timer += Time.deltaTime;
            yield return null;
        }
    }


    private void OnDestroy()
    {

    }
}
