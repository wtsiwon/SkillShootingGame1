using System.Collections;
using System.Collections.Generic;
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

    private bool isPatternDone = true;

    private List<IEnumerator> IBossPatternList = new List<IEnumerator>(5);

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
        base.Start();
        StartCoroutine(IBossMove());
        isBossMove = true;
        IsDie = false;

        CoroutineSubstitution();
    }

    /// <summary>
    /// 코루틴 대입
    /// </summary>
    private void CoroutineSubstitution()
    {

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

    private void RandomBossPattern()
    {
        if (isPatternDone == true)
        {
            int randPattern = Random.Range(1, 4);
            print("BossPattern" + randPattern);
            StartCoroutine($"IBossPattern{randPattern}");
        }
        //StartCoroutine(IBossPatternList[randPattern]);
    }

    private IEnumerator IBossPattern1()
    {
        isPatternDone = false;

        int angle = 360 / 12;


        for (int j = 0; j < 5; j++)
        {
            int count = 0;
            for (int i = 0; i < 360; i += angle)
            {
                Quaternion rot = Quaternion.Euler(new Vector3(0, 0, i + count * 15 + 90));

                Bullet bullet1 = ObjPool.Instance.GetBullet(transform.position, 4, rot);

                bullet1.SetBullet(transform.position, rot, bulletSpd, dmg, true, 4);
            }
            count++;
            yield return new WaitForSeconds(1f);
        }

        isPatternDone = true;
        yield break;
    }
    /// <summary>
    /// 흩 뿌리기
    /// </summary>
    /// <param name="bulletCount"></param>
    /// <param name="rotate"></param>
    /// <param name="rotateCount"></param>
    /// <param name="entireCount"></param>
    /// <returns></returns>
    private IEnumerator IBossPattern2()
    {
        isPatternDone = false;
        int rotate = 10;
        int rotateCount = 3;
        int entireCount = 5;
        int central = 0;

        float z = -rotate;
        float amount = (rotate * 2f) / rotateCount;

        for (int i = 0; i < 8; i++)
        {
            Vector2 nor = (Vector3.zero - transform.position).normalized;
            float tarZ = Mathf.Atan2(nor.y, nor.x) * Mathf.Rad2Deg;


            z += amount;
            if (z == rotate || z == -rotate)
            {
                amount *= -1;
            }

            ShotBullet(central, z + tarZ, entireCount);

            yield return new WaitForSeconds(0.1f);
        }

        void ShotBullet(float central, float startRot, int bulletCount)
        {
            float amount = central / (bulletCount - 1);
            float z = central / -2f;

            for (int i = 0; i < bulletCount; i++)
            {
                Quaternion rot = Quaternion.Euler(0, 0, z + startRot);

                Bullet bullet = ObjPool.Instance.GetBullet(transform.position, 4, rot);

                bullet.SetBullet(transform.position, rot, bulletSpd, dmg, true, 4);
                z += amount;
            }
        }
        isPatternDone = true;
        yield break;
    }

    private IEnumerator IBossPattern4()
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
            Quaternion rot = Quaternion.Euler(Vector3.up);
            Bullet bullet1 = ObjPool.Instance.GetBullet(transform.position, 4, rot);

            bullet1.SetBullet(transform.position, rot, bulletSpd, dmg, true, 4);

            transform.rotation = Quaternion.Euler(0, 0, transform.rotation.z + 5);

            time += Time.deltaTime;
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

    private IEnumerator IBossPattern3()
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

        while (percent < 1)
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
        print("Boss");
        RandomBossPattern();
    }
    protected override void OnDie()
    {
        if (isBoss == true) BossDie();
    }

}