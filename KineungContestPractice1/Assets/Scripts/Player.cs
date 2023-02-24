using System.Collections;
using System.Collections.Generic;
using UnityEditor.U2D.Sprites;
using UnityEngine;
using UnityEngine.UIElements;

public class Player : Singleton<Player>
{
    public float moveSpd;

    public float atkSpd;

    public float bulletSpd;

    public float atkDmg;

    [Space(10f)]
    public bool isShoot;

    [SerializeField]
    private int maxLevel;

    [Space(10f)]
    [SerializeField]
    private int level;
    public int Level
    {
        get => level;

        set
        {
            if (value >= maxLevel) level = maxLevel;
            else level = value;
        }
    }

    public List<Vector3> petPosList = new List<Vector3>();

    public int maxPetCount;

    private int petCount;

    public int PetCount
    {
        get => petCount;
        set
        {
            petCount = value;

        }
    }

    public Vector3 clampPosition;

    public Bullet bullet;

    [SerializeField]
    [Space(10f)]
    private bool isInvicibility;
    public bool IsInvicibility
    {
        get => isInvicibility;
        set
        {
            GetComponent<Collider2D>().enabled = !isInvicibility;
        }
    }

    private Color fadeColor = new Color(255, 255, 255, 140);
    private Color notFadeColor = Color.white;

    private SpriteRenderer spriteRenderer;

    [SerializeField]
    [Space(10f)]
    private int maxHp;

    [SerializeField]
    private int hp;
    public int Hp
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
                StartCoroutine(Invicibility(3));
            }

            GameManager.Instance.UpdatePlayerHpIcon(Hp);

        }
    }

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        StartCoroutine(IShoot());
        StartCoroutine(IUpdate());
    }

    private IEnumerator IUpdate()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.5f);
        }
    }

    void Update()
    {
        Move();
        InputShootKey();
        transform.position = ClampPosition();
    }

    private void Move()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        Vector3 dir = new Vector3(x, y, 0);

        transform.Translate(dir * moveSpd * Time.deltaTime);
    }

    private void InputShootKey()
    {
        if (Input.GetKey(KeyCode.Z))
        {
            isShoot = true;
        }
        else
        {
            isShoot = false;
        }
    }

    private IEnumerator IShoot()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.01f);
            if (isShoot == true)
            {
                switch (level)
                {
                    case 1:
                        Bullet bullet1 = Instantiate(bullet);
                        bullet1.SetBullet(transform.position, Vector3.up, bulletSpd, level, atkDmg, false);
                        break;
                    case 2:
                        Bullet bullet2 = Instantiate(bullet);
                        bullet2.SetBullet(new Vector3(transform.position.x + 0.5f, transform.position.y, 0),
                            Vector3.up, bulletSpd, level, atkDmg, false);

                        Bullet bullet3 = Instantiate(bullet);
                        bullet3.SetBullet(new Vector3(transform.position.x - 0.5f, transform.position.y, 0),
                            Vector3.up, bulletSpd, level, atkDmg, false);
                        break;
                    default:
                        Bullet bullet4 = Instantiate(bullet);
                        bullet4.SetBullet(new Vector3(transform.position.x + 0.5f, transform.position.y, 0),
                            Vector3.up, bulletSpd, level, atkDmg, false);

                        Bullet bullet5 = Instantiate(bullet);
                        bullet5.SetBullet(new Vector3(transform.position.x - 0.5f, transform.position.y, 0),
                            Vector3.up, bulletSpd, level, atkDmg, false);
                        break;
                }
                yield return new WaitForSeconds(atkSpd);
            }

        }
    }

    #region ㅎㅎ
    //private IEnumerator IGuideShoot(int count)
    //{
    //    Enemy[] targets = FindTargets();

    //    if (targets.Length == 0) yield break;

    //    int manyShoot = count / targets.Length;//하나의 적에게 몇개의 총알을 쏠것인가

    //    if(count < targets.Length) //총알의 수보다 적이 더 많을 때
    //        manyShoot = 1;

    //    float interval = 2 / count;
    //    for (int i = 0; i < count; i++)
    //    {
    //        for (int j = 0; j < manyShoot; j++)
    //        {
    //            BezierBullet bullet1 = Instantiate(bezierBullet);
    //            float randomX = Random.Range(-1.0f, 1.0f);
    //            Vector3 pos = new Vector3(randomX, transform.position.y, 0);
    //            bullet1.SetBullet(pos, bulletSpd / 10, atkDmg, target);
    //        }
    //        yield return new WaitForSeconds(interval);
    //    }
    //}
    #endregion

    private Vector3 ClampPosition()
    {
        Vector3 vector3 = new Vector3();

        vector3.x = Mathf.Clamp(transform.position.x, -clampPosition.x, clampPosition.x);
        vector3.y = Mathf.Clamp(transform.position.y, -clampPosition.y, clampPosition.y);
        vector3.z = 0;
        return vector3;
    }

    /// <summary>
    /// 파트너 추가
    /// </summary>
    public void AddPartner()
    {

    }

    public void OnDamaged()
    {
        Hp -= 1;
    }

    private IEnumerator Invicibility(float time)
    {
        float interval = time / 7f;
        float count = 0;

        while (count == 7)
        {
            if (spriteRenderer.color == fadeColor)
            {
                spriteRenderer.color = notFadeColor;
                count++;
            }
            else
            {
                spriteRenderer.color = fadeColor;
                count++;
            }
            yield return new WaitForSeconds(interval);
        }
    }

    private void OnDie()
    {

    }
}
