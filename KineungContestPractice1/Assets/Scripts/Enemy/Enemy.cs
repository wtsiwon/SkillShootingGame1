using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public abstract class Enemy : MonoBehaviour
{
    public bool isBoss;

    public float moveSpd;

    public int score;//잡으면 주는 점수

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
    private float hp;
    public virtual float Hp
    {
        get
        {
            return hp;
        }
        set
        {
            if (value <= 0) OnDie();
            else if (value > maxHp) hp = maxHp;
            else
            {
                hp = value;
                StartCoroutine(IOnDamaged());
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
    }

    private void Update()
    {
        if (isMove == true)
        {
            transform.Translate(Vector3.down * moveSpd * Time.deltaTime);
        }
    }

    public void OnDamaged(float dmg)
    {
        Hp -= dmg;
        print(dmg);
    }

    private IEnumerator IOnDamaged()
    {
        onDmgObj.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        onDmgObj.SetActive(false);
    }

    protected virtual void OnDie()
    {
        GameManager.Instance.GetDestroyEffect(transform.position, 15);
        GameManager.Instance.Score += score;
        if (isBoss == false)
        {
            ItemSpawn();
            Destroy(gameObject);
        }
    }

    private void ItemSpawn()
    {
        int rand = Random.Range(0, 10);
        if(rand == 0)
        {
            GameManager.Instance.SpawnRandomItem(transform.position);
        }
    }


    protected virtual IEnumerator IAttack()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.01f);
            if (isAttack == true)
            {
                yield return new WaitForSeconds(atkSpd);
                Attack();
            }
        }
    }

    protected abstract void Attack();
}