using System.Collections;
using UnityEngine;

public class Boss : Enemy
{
    [SerializeField]
    private Meteor meteor;

    [SerializeField]
    private Transform shootingPos;

    public bool isBossMove;

    [SerializeField]
    private float shotInverval;

    [SerializeField]
    private Vector3 maxPosition;

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

            if (hp == maxHp / 2)
            {
                for (int i = 0; i < 5; i++)
                {
                    Vector3 pos = Random.insideUnitCircle * 2;
                    GameManager.Instance.SpawnRandomItem(transform.position + pos);
                }
            }
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

    protected override IEnumerator IAttack()
    {
        return base.IAttack();
    }

    private void RandomBossPattern()
    {
        int randPattern = Random.Range(1, 2);

        print("PlayRandomPattern");
        StartCoroutine($"IBossPattern{5}");
        
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

                Quaternion rot = Quaternion.Euler(new Vector3(0, 0, i + count * 15 + 90));
                bullet1.SetBullet(transform.position, rot, bulletSpd, 1, true);
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

    private IEnumerator IBossPattern3()
    {
        isPatternDone = false;
        StartCoroutine(IPattern3Shoot(atkSpd - 1));

        isPatternDone = true;

        yield return null;
    }

    private IEnumerator IPattern3Shoot(float duration)
    {
        float time = 0;
        while (time < duration)
        {
            Bullet bullet1 = Instantiate(bullet);
            Quaternion rot = Quaternion.Euler(Vector3.up);

            bullet1.SetBullet(transform.position, rot, bulletSpd, dmg, true);

            transform.rotation = Quaternion.Euler(0, 0, transform.rotation.x + 5);

            time = Time.deltaTime;
            yield return null;
        }

        time = 0;
        while (time < 0.5f)
        {
            transform.rotation = Quaternion.Lerp(Quaternion.Euler(0, 0, transform.rotation.z), Quaternion.Euler(Vector3.zero), time / 0.5f);
            yield return null;
        }
        yield break;
    }

    private IEnumerator IPattern4Shoot(float duration)
    {
        float time = 0;
        while (time < duration)
        {
            //꽃모양 어쩌고
            time = Time.deltaTime;

        }
        yield break;
    }

    private IEnumerator IBossPattern5()
    {
        isPatternDone = false;
        GameObject warning = GameManager.Instance.warningArea;

        float x = Random.Range(-maxPosition.x, maxPosition.x);
        float y = Random.Range(-maxPosition.y, maxPosition.y);

        warning.transform.position = new Vector3(x, y, 0);

        for (int i = 0; i < 3; i++)
        {
            StartCoroutine(IFadeInOut(1, warning));
            yield return null;
        }

        float current = 0;
        float percent = 0;

        Vector3 startPos = transform.position;
        Vector3 endPos = new Vector3(x, y, 0);
        Vector3 currentPos;

        while (percent < 1)
        {
            current += Time.deltaTime;
            percent = current / 1f;

            currentPos = Vector3.Lerp(startPos, endPos, percent);
            transform.position = currentPos;

            yield return null;
        }

        yield return new WaitForSeconds(0.5f);

        current = 0;
        percent = 0;

        while(percent < 1)
        {
            current += Time.deltaTime;
            percent = (current / 1f);

            currentPos = Vector3.Lerp(endPos, startPos, percent);
            transform.position = currentPos;
            yield return null;
        }

        isPatternDone = true;
        yield break;
    }

    private IEnumerator IFadeInOut(float time, GameObject warning)
    {
        float current = 0;
        float percent = 0;
        Color tempColor = warning.GetComponent<SpriteRenderer>().color;


        while (percent < 1)
        {
            current += Time.deltaTime;
            percent = current / (time * 0.5f);

            tempColor.a = Mathf.Lerp(0, 1, percent);

            warning.GetComponent<SpriteRenderer>().color = tempColor;
            yield return null;
        }

        yield return new WaitForSeconds(0.5f);

        current = 0;
        percent = 0;

        while (percent < 1)
        {
            current += Time.deltaTime;
            percent = current / (time * 0.5f);

            tempColor.a = Mathf.Lerp(1, 0, percent);

            warning.GetComponent<SpriteRenderer>().color = tempColor;
            yield return null;
        }

        isPatternDone = true;
        yield break;
    }




    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Player player))
        {
            player.OnDamaged(dmg);
        }
    }

    protected override void Attack()
    {
        print("?");
        RandomBossPattern();
    }
    protected override void OnDie()
    {
        if (isBoss == true) BossDie();
    }

}