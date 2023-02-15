using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UIElements;

public class Boss : Enemy
{
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

    protected override void Start()
    {
        base.Start();
    }

    void Update()
    {
        if (IsDie == true)
        {
            transform.Translate(Vector3.up * Time.deltaTime);
        }
    }

    protected override void OnDie()
    {
        base.OnDie();
        BossDie();
    }

    private void BossDie()
    {
        StartCoroutine(IBossDieEffect());
    }

    private IEnumerator IBossDieEffect()
    {
        int count = 0;
        while (count > 20)
        {
            yield return new WaitForSeconds(0.1f);
            count++;
            GameObject obj = Instantiate(GameManager.Instance.destroyEffect, RandomPositon(transform.position, 4, 3), Quaternion.identity);
            obj.transform.localScale = new Vector3(15, 15, 1);
        }

    }

    private Vector3 RandomPositon(Vector3 originPos, float xRange, float yRange)
    {
        float x = Random.Range(-xRange, xRange);
        float y = Random.Range(-yRange, 0);
        Vector3 pos = new Vector3(originPos.x + x, originPos.y + y, originPos.z);

        return pos;
    }

    private IEnumerator IDyingMove()
    {
        yield return new WaitForSeconds(3);
    }
}
