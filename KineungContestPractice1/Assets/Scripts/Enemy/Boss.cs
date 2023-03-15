using System.Collections;
using UnityEngine;

public class Boss : Enemy
{
    [SerializeField]
    private Transform shootingPos;

    public bool isBossMove;

    public int patternNum;

    private bool isPatternDone;

    public override float Hp
    {
        get
        {
            return hp;
        }
        set
        {
            base.Hp = value;
        }
    }


    protected override void Start()
    {
        StartCoroutine(IBossMove());
        isBossMove = true;
        IsDie = false;
    }

    protected void Update()//일부러 상속 안받음
    {
        if (IsDie == true)
        {
            transform.Translate(Vector3.up * moveSpd * Time.deltaTime);
        }
    }

    private IEnumerator IUpdate()
    {
        while (true)
        {
            if (isPatternDone == true)
            {
                //RandomBossPattern();
            }
            yield return null;
        }
    }

    protected override IEnumerator IAttack()
    {
        return base.IAttack();
    }

    private void BossDie()
    {
        if (isDie == false)
        {
            isDie = true;
            EnemySpawner.Instance.isBossSpawned = false;
            StartCoroutine(nameof(IBossDieEffect));
            Destroy(gameObject, 5f);
        }
    }

    private IEnumerator IBossDieEffect()
    {
        print("DieEffect");
        int count = 0;
        while (count < 100)
        {
            yield return new WaitForSeconds(0.03f);
            count++;
            GameObject obj = Instantiate(GameManager.Instance.destroyEffect, RandomPositon(transform.position, 4, 3), Quaternion.identity);
            obj.transform.localScale = new Vector3(15, 15, 1);
        }

        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }

    private Vector3 RandomPositon(Vector3 originPos, float xRange, float yRange)
    {
        float x = Random.Range(-xRange, xRange);
        float y = Random.Range(-yRange, 0);
        Vector3 pos = new Vector3(originPos.x + x, originPos.y + y, originPos.z);

        return pos;
    }

    private IEnumerator IBossMove()
    {
        Vector3 pos1 = new Vector3(0, 7, 0);
        Vector3 pos2 = new Vector3(0, 13, 0);

        float duration = 2;
        float timer = 0;

        while (timer < duration)
        {
            timer += Time.deltaTime;

            transform.position = Vector3.Lerp(pos2, pos1, timer / duration);
            yield return null;
        }

        isBossMove = false;
    }

    private void RandomBossPattern()
    {
        int randPattern = Random.Range(1, 2);

        StartCoroutine($"IBossPattern{1}");
    }

    private IEnumerator IBossPattern1()
    {
        isPatternDone = false;

        int angle = 180 / 12;


        for (int j = 0; j < 10; j++)
        {
            int count = 0;
            for (int i = 0; i < 180; i += angle)
            {
                Bullet bullet1 = Instantiate(bullet, transform.position, Quaternion.identity);
                bullet1.SetBullet(transform.position, new Vector3(0, 0, i + count * 15 + 90), bulletSpd, 1, true);
            }
            count++;
            yield return new WaitForSeconds(1f);
        }

        isPatternDone = true;
        yield break;
    }


    private IEnumerator IBossPattern2(int bulletCount, int central, int rotate, int rotateCount, int entireCount)
    {
        isPatternDone = false;

        float z = -rotate;
        float amount = (rotate * 2f) / rotateCount;

        for (int i = 0; i < entireCount; i++)
        {
            Vector2 nor = (Vector3.zero - transform.position).normalized;
            float tarZ = Mathf.Atan2(nor.y, nor.x) * Mathf.Rad2Deg;


            z += amount;
            if (z == rotate || z == -rotate)
            {
                amount *= -1;
            }

            ShotBullet(central, z + tarZ, bulletCount);

            yield return new WaitForSeconds(0.1f);
        }

        void ShotBullet(float central, float startRot, int bulletCount)
        {
            float amount = central / (bulletCount - 1);
            float z = central / -2f;

            for (int i = 0; i < bulletCount; i++)
            {
                Quaternion rot = Quaternion.Euler(0, 0, z + startRot);
                Instantiate(bullet, transform.position, rot);
                z += amount;
            }
        }
        isPatternDone = true;
        yield break;
    }



    protected override void Attack()
    {
        RandomBossPattern();
    }
    protected override void OnDie()
    {
        if (isBoss == true) BossDie();
    }

}