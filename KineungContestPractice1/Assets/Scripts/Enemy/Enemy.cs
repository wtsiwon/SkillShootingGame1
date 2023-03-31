using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public abstract class Enemy : MonoBehaviour
{
    public bool isBoss;

    public float moveSpd;

    public int score;//잡으면 주는 점수

    public float dmg;

    public float atkSpd;//공격속도

    public bool isAttack;

    public float bulletSpd;

    public Bullet bullet;

    protected bool isDie;
    public virtual bool IsDie
    {
        get
        {
            return isDie;
        }
        set
        {
            isDie = value;
        }
    }

    public float maxHp;

    [SerializeField]
    protected float hp;
    public virtual float Hp
    {
        get
        {
            return hp;
        }
        set
        {
            if (value <= 0)
            {
                OnDie();
                StartCoroutine(nameof(IOnDamaged));
            }
            else if (value > maxHp) hp = maxHp;
            else
            {
                hp = value;
                StartCoroutine(nameof(IOnDamaged));
            }
        }
    }

    public bool isMove = true;

    [SerializeField]
    protected GameObject onDmgObj;

    protected SpriteRenderer spriteRenderer;

    protected virtual void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        StartCoroutine(nameof(IAttack));
        SetEnemy();
    }

    protected void SetEnemy()
    {
        maxHp = maxHp * GameManager.stage;
        hp = maxHp;
    }

    protected virtual void Update()
    {
        if (isMove == true)
        {
            transform.Translate(Vector3.down * moveSpd * Time.deltaTime);
        }

        EnemyDestroy();
    }

    public void OnDamaged(float dmg)
    {
        Hp -= dmg;
    }

    protected IEnumerator IOnDamaged()
    {
        onDmgObj.SetActive(true);
        yield return new WaitForSeconds(0.05f);
        onDmgObj.SetActive(false);
    }

    protected virtual void OnDie()
    {
        if (isBoss == false) EnemyDie();
    }

    protected void EnemyDie()
    {
        GameManager.Instance.GetDestroyEffect(transform.position, 15);
        GameManager.Instance.Score += score;
        ItemSpawn();
        Destroy(gameObject);
    }

    protected void EnemyDestroy()
    {
        float distance = Vector3.Distance(Vector3.zero, transform.position);
        if (distance > GameManager.Instance.destroyDistance)
        {
            Destroy(gameObject);
        }
    }

    private void ItemSpawn()
    {
        int rand = Random.Range(0, 10);
        //아이템 드롭 확률 50%
        if (rand >= 5)
        {
            GameManager.Instance.SpawnRandomItem(transform.position);
        }
    }


    protected virtual IEnumerator IAttack()
    {
        var wait = new WaitForSeconds(0.01f);

        while (true)
        {
            yield return wait;
            if (isAttack == true)
            {
                yield return new WaitForSeconds(atkSpd);
                Attack();
            }
        }
    }
    protected abstract void Attack();
}