using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class BezierBullet : Bullet
{
    [Range(0, 3f)]
    public float duration;
    private float t;

    private float startDistance;
    private float endDistance;

    private Vector3[] points = new Vector3[4];

    public Transform target;


    void Start()
    {
        StartCoroutine(BezierCurve());
    }

    void Update()
    {
        
    }

    private IEnumerator BezierCurve()
    {
        yield return new WaitForSeconds(0.005f);

        while (t < duration)
        {
            MoveBezier(t / duration);

            t += Time.deltaTime * moveSpd;
            yield return null;
        }
        Destroy(gameObject);
    }

    public void SetBullet(Vector2 pos, float moveSpd, float dmg, Transform target, float startDistance, float endDistance, bool isEnemyBullet)
    {
        transform.position = pos;
        this.moveSpd = moveSpd;
        this.dmg = dmg;
        this.target = target;
        AssignFourPoint();
        this.isEnemyBullet = isEnemyBullet;
    }


    private float FourPointBezier(float a, float b, float c, float d, float t)
    {
        float a2b = Mathf.Lerp(a, b, t);
        float b2c = Mathf.Lerp(b, c, t);
        float c2d = Mathf.Lerp(c, d, t);

        float ab2dc = Mathf.Lerp(a2b, b2c, t);
        float bc2cd = Mathf.Lerp(b2c, c2d, t);

        float f = Mathf.Lerp(ab2dc, bc2cd, t);

        return f;
    }

    public void AssignFourPoint()
    {
        points[0] = transform.position;

        points[1] = PointSetting(transform.position) + 
            (Vector2)(startDistance * Random.Range(-1.0f, 1.0f) * transform.right) + 
            (Vector2)(startDistance * Random.Range(-0.15f, 1.0f) * transform.up);

        points[2] = PointSetting(target.position) + 
            (Vector2)(endDistance * Random.Range(-1.0f, 1.0f) * target.right) + 
            (Vector2)(endDistance * Random.Range(-0.15f, 1.0f) * target.up);

        points[3] = target.position;

    }

    private Vector2 PointSetting(Vector2 origin)
    {
        float x, y;

        x = 0.45f * Mathf.Cos(Random.Range(0, 360) * Mathf.Deg2Rad) + origin.x;
        y = 0.55f * Mathf.Sin(Random.Range(0, 360) * Mathf.Deg2Rad) + origin.y;

        return new Vector2(x, y);
    }

    #region ?

    //m_points[1] = _startTr.position +
    //        (_newPointDistanceFromStartTr* Random.Range(-1.0f, 1.0f) * _startTr.right) + // X (좌, 우 전체)
    //        (_newPointDistanceFromStartTr* Random.Range(-0.15f, 1.0f) * _startTr.up) + // Y (아래쪽 조금, 위쪽 전체)
    //        (_newPointDistanceFromStartTr* Random.Range(-1.0f, -0.8f) * _startTr.forward); // Z (뒤 쪽만)

    //    // 도착 지점을 기준으로 랜덤 포인트 지정.
    //    m_points[2] = _endTr.position +
    //        (_newPointDistanceFromEndTr* Random.Range(-1.0f, 1.0f) * _endTr.right) + // X (좌, 우 전체)
    //        (_newPointDistanceFromEndTr* Random.Range(-1.0f, 1.0f) * _endTr.up) + // Y (위, 아래 전체)
    //        (_newPointDistanceFromEndTr* Random.Range(0.8f, 1.0f) * _endTr.forward); // Z (앞 쪽만)
    #endregion

    private void MoveBezier(float t)
    {
        transform.position = new Vector2(
            FourPointBezier(points[0].x, points[1].x, points[2].x, points[3].x,t),
            FourPointBezier(points[0].y, points[1].y, points[2].y, points[3].y, t)
            );
    }

    //private float FourPointBezier(float a, float b, float c, float d)
    //{
    //    return Mathf.Pow((1 - t), 3) * a
    //        + Mathf.Pow((1 - t), 2) * 3 * t * b
    //        + Mathf.Pow(t, 2) * 3 * (1 - t) * c
    //        + Mathf.Pow(t, 3) * d;
    //}
}
