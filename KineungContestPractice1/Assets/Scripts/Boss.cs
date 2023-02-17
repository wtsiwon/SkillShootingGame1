using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UIElements;

public class Boss : Enemy
{
    public bool isBossMove;

    public int patternNum;

    private bool isDie;
    public bool IsDie
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

    public override float Hp
    {
        get => base.Hp;
        set
        {
            base.Hp = value;
            if (value <= 0)
            {
                OnDie();
            }
        }
    }


    protected override void Start()
    {
        base.Start();
        StartCoroutine(IBossMove());
        isBossMove = true;
    }

    void Update()
    {
        if (IsDie == true)
        {
            transform.Translate(Vector3.up * moveSpd * Time.deltaTime);
        }
    }

    protected override void OnDie()
    {
        base.OnDie();
        BossDie();
    }

    private void BossDie()
    {
        isDie = true;
        EnemySpawner.Instance.isBossSpawned = false;
        StartCoroutine(IBossDieEffect());
    }

    private IEnumerator IBossDieEffect()
    {
        print("DieEffect");
        int count = 0;
        while (count < 30)
        {
            yield return new WaitForSeconds(0.1f);
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

    protected override void Attack()
    {
        RandomBossPattern();
    }

    private void RandomBossPattern()
    {
        int randPattern = Random.Range(1, 2);

        StartCoroutine($"IBossPattern{randPattern}");
    }

    private IEnumerator IBossPattern1()
    {
        int angle = 360 / 12;

        for (int i = 0; i < 12; i += angle)
        {
            GameObject obj = Instantiate(bullet, transform.position, Quaternion.identity).gameObject;
            obj.transform.rotation = Quaternion.Euler(new Vector3(0, 0, i));
        }
        yield return null;
    }
}
