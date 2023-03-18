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

    [SerializeField]
    protected bool isEnemyBullet;

    [SerializeField]
    protected float moveSpd;

    private Vector3 dir = Vector3.up;

    protected float dmg;
    public virtual float Dmg
    {
        get
        {
            return dmg;
        }
    }

    public float scale;

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
        Destroy();
    }

    public void SetBullet(Vector3 pos, Vector3 dir, float spd, float dmg, bool isEnemyBullet, bool isMoving = true, bool isGuided = false)
    {
        transform.position = pos;
        moveSpd = spd;
        isMove = isMoving;
        transform.rotation = Quaternion.Euler(dir);
        this.dmg = dmg;
        this.isEnemyBullet = isEnemyBullet;
        this.isGuided = isGuided;
    }

    public void SetScale(float scale)
    {
        this.scale = scale;
    }

    private void Destroy()
    {
        float distance = Vector3.Distance(Vector3.zero, transform.position);
        if (distance > GameManager.Instance.destroyDistance)
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
                player.OnDamaged((int)Dmg);
                GameManager.Instance.GetDestroyEffect(transform.position);
                Destroy(gameObject);
            }
        }
        else
        {
            if (collision.TryGetComponent<Enemy>(out Enemy enemy))
            {
                enemy.OnDamaged(Dmg);
                GameManager.Instance.GetDestroyEffect(transform.position, scale, true);
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
