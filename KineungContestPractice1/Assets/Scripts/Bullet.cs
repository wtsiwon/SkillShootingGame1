using System.Collections;
using System.Collections.Generic;
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
            if(value == true)
            {
                StartCoroutine(nameof(IGuidShoot));
            }
        }
    }

    private bool isTargetChasing;


    private Transform target;

    [SerializeField]
    private ContactFilter2D filter;

    [SerializeField]
    private Collider2D col;

    private bool isEnemyBullet;

    private float moveSpd;

    private int level;

    private Vector3 dir = Vector3.up;

    private float dmg;
    public float Dmg
    {
        get
        {
            return dmg * level;
        }
    }

    private void Start()
    {
        
    }

    private void Update()
    {
        if (isMove == true)
        {
            transform.Translate(dir * moveSpd * Time.deltaTime);
        }
        if(isTargetChasing == true)
        {
            LookTarget();
        }
        Destroy();
    }

    public void SetBullet(Vector3 pos, Vector3 dir, float spd, int level, float dmg, bool isEnemyBullet, bool isMoving = true, bool isGuided = false)
    {
        transform.position = pos;
        moveSpd = spd;
        isMove = isMoving;
        this.dir = dir;
        this.dmg = dmg;
        this.level = level;
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

    private void OnTriggerEnter2D(Collider2D collision)
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
        print(collision);
    }

    private IEnumerator IGuidShoot()
    {
        Quaternion startRot = transform.rotation;
        Quaternion endRot = new Quaternion(0, 0, 0, 0);

        float duration = 0.5f;
        float timer = 0;
        while (timer < duration)
        {
            transform.rotation = Quaternion.Lerp(startRot, endRot, timer / duration);

            timer += Time.deltaTime;
            yield return null;
        }
        StartCoroutine(nameof(FindTarget));
    }

    private IEnumerator FindTarget()
    {
        while(target != null)
        {
            yield return null;
            List<Collider2D> colList = new List<Collider2D>();

            Physics2D.OverlapCollider(col, filter, colList);

            if (colList.Count == 0) continue;

            target = colList[0].transform;
        }
    }

    private void LookTarget()
    {
        dir = (target.position - transform.position).normalized;
    }

    private void OnDestroy()
    {
        print(Dmg);
    }
}
