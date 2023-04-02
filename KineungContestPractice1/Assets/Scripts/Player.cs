using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using System;

public enum EShootPos
{
    Left,
    Center,
    Right,
}

public class Player : Singleton<Player>
{
    public float moveSpd;

    public float atkSpd;

    public float bulletSpd;

    public float atkDmg;

    [Space(10f)]
    public bool isShoot;

    [Space(5f)]
    private bool isSlowMove;

    public readonly int maxLevel = 7;
    [SerializeField]
    private int level;
    public int Level
    {
        get => level;

        set
        {
            if (value >= maxLevel) level = maxLevel;
            else
            {
                level = value;
                GameManager.Instance.UpdatePlayerLevelText(level);
            }
        }
    }

    public List<Pet> petList = new List<Pet>();
    public List<Vector3> petPosList = new List<Vector3>();

    public int maxPetCount;

    private int petCount;

    public int PetCount
    {
        get => petCount;
        set
        {
            petCount = value;
            AddPet(value);
        }
    }

    public Vector3 clampPosition;

    public List<Bullet> bulletList = new List<Bullet>();

    [Tooltip("총알레벨에 따른 추가 데미지")]
    public List<float> levelPerbulletAdditionalDmg = new List<float>();

    [Tooltip("총알 발사 위치")]
    public List<Vector3> bulletShootPosList = new List<Vector3>();


    [Tooltip("무적시간")]
    public float invicibilityTime;

    [SerializeField]
    [Space(10f)]
    private bool isInvicibility;

    [SerializeField]
    private GameObject invicibilityCircle;
    public bool IsInvicibility
    {
        get => isInvicibility;
        set
        {
            isInvicibility = value;
            invicibilityCircle.SetActive(value);
        }
    }

    private Color fadeColor = new Color(255, 255, 255, 140);
    private Color notFadeColor = Color.white;

    private SpriteRenderer spriteRenderer;

    [Space(10f)]
    public readonly int maxHp = 100;

    private float hp;
    public float Hp
    {
        get
        {
            return hp;
        }
        set
        {
            float temp = hp;

            hp = value;

            if (temp > value)
            {
                GameManager.Instance.CameraShake(0.5f, 0.2f);
            }
            if (value <= 0)
            {
                if (isInvicibility == true) return;
                OnDie();
            }
            else if (value > maxHp) hp = maxHp;
            else
            {
                if (isInvicibility == true)
                {
                    return;
                }

                StartCoroutine(IInvicibility());
            }

            GameManager.Instance.UpdatePlayerHpBar(hp / maxHp);
        }
    }

    [Space(10f)]
    [Tooltip("연료 감소 속도")]
    public float decrease;

    [SerializeField]
    private float maxFuel;

    private float fuel;

    public float Fuel
    {
        get => fuel;
        set
        {
            if (value <= 0) OnDie();
            else if (value > maxFuel) fuel = maxFuel;
            else
            {
                fuel = value;
            }
            GameManager.Instance.UpdatePlayerFuelBar(Fuel / maxFuel);
        }
    }



    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        SetPlayer();

        StartCoroutine(IShoot());
        StartCoroutine(IUpdate());
        StartCoroutine(IFuelReduction());

    }

    private void SetPlayer()
    {
        fuel = maxFuel;
        hp = maxHp;
        petCount = 0;
        GameManager.Instance.UpdatePlayerLevelText(1);
    }

    private IEnumerator IUpdate()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.5f);
        }
    }

    private IEnumerator IFuelReduction()
    {
        while (Fuel > 0)
        {
            yield return new WaitForSeconds(0.05f);
            Fuel -= decrease;
        }
    }

    private IEnumerator IInvicibility()
    {
        IsInvicibility = true;
        yield return new WaitForSeconds(invicibilityTime);
        IsInvicibility = false;
    }

    void Update()
    {
        Move();
        InputShootKey();
        transform.position = ClampPosition();

        if (Input.GetKeyDown(KeyCode.F2))
        {
            Hp += 10f;
        }

    }

    private void Move()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        Vector3 dir = new Vector3(x, y, 0);

        if (isSlowMove == true)
        {
            transform.Translate(dir * moveSpd / 2 * Time.deltaTime);
        }
        else
        {
            transform.Translate(dir * moveSpd * Time.deltaTime);
        }
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

        if (Input.GetKey(KeyCode.LeftShift))
        {
            isSlowMove = true;
        }
        else
        {
            isSlowMove = false;
        }
    }

    private IEnumerator IShoot()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.01f);
            if (isShoot == true)
            {
                StartCoroutine($"IShootingPattern{level}");
                yield return new WaitForSeconds(atkSpd);
            }
        }
    }

    private Bullet GetBullet(int bulletLevel)
    {
        Bullet bullet = Instantiate(bulletList[bulletLevel - 1]);

        Quaternion rot = Quaternion.Euler(Vector3.up);

        bullet.SetBullet(transform.position, rot, bulletSpd, atkDmg + (atkDmg * levelPerbulletAdditionalDmg[level - 1]), false);

        return bullet;
    }

    #region PlayerShooting
    //레벨에 따른 탄막
    private IEnumerator IShootingPattern1()
    {
        Bullet bullet = GetBullet(1);
        Vector3 bulletPos = transform.position;
        bulletPos += bulletShootPosList[(int)EShootPos.Center];
        bullet.transform.position = bulletPos;
        yield break;
    }

    private IEnumerator IShootingPattern2()
    {
        Bullet bullet1 = GetBullet(1);
        Vector3 bullet1Pos = transform.position;
        bullet1Pos += bulletShootPosList[(int)EShootPos.Left];
        bullet1.transform.position = bullet1Pos;

        Bullet bullet2 = GetBullet(1);
        Vector3 bullet2Pos = transform.position;
        bullet2Pos += bulletShootPosList[(int)EShootPos.Right];
        bullet2.transform.position = bullet2Pos;
        yield break;
    }

    private IEnumerator IShootingPattern3()
    {
        Bullet bullet = GetBullet(2);
        Vector3 bulletPos = transform.position;
        bulletPos += bulletShootPosList[(int)EShootPos.Center];
        bullet.transform.position = bulletPos;
        yield break;
    }

    private IEnumerator IShootingPattern4()
    {
        Bullet bullet1 = GetBullet(1);//왼쪽
        Vector3 bullet1Pos = transform.position;
        bullet1Pos += bulletShootPosList[(int)EShootPos.Left];
        bullet1.transform.position = bullet1Pos;

        StartCoroutine(IShootingPattern3());

        Bullet bullet2 = GetBullet(1);//오른쪽
        Vector3 bullet2Pos = transform.position;
        bullet2Pos += bulletShootPosList[(int)EShootPos.Right];
        bullet2.transform.position = bullet2Pos;

        yield break;
    }

    private IEnumerator IShootingPattern5()
    {
        Bullet bullet1 = GetBullet(2);
        Vector3 bullet1Pos = transform.position;
        bullet1Pos += bulletShootPosList[(int)EShootPos.Left];
        bullet1.transform.position = bullet1Pos;

        Bullet bullet2 = GetBullet(1);
        Vector3 bullet2Pos = transform.position;
        bullet2Pos += bulletShootPosList[(int)EShootPos.Center];
        bullet2.transform.position = bullet2Pos;

        Bullet bullet3 = GetBullet(2);
        Vector3 bullet3Pos = transform.position;
        bullet3Pos += bulletShootPosList[(int)EShootPos.Right];
        bullet3.transform.position = bullet3Pos;

        yield break;
    }

    private IEnumerator IShootingPattern6()
    {
        Bullet bullet0 = GetBullet(1);
        Vector3 bullet0Pos = transform.position;
        bullet0Pos += bulletShootPosList[(int)EShootPos.Left];
        bullet0.transform.position = bullet0Pos;

        Bullet bullet1 = GetBullet(3);
        Vector3 bullet1Pos = transform.position;
        bullet1Pos += bulletShootPosList[(int)EShootPos.Center];
        bullet1.transform.position = bullet1Pos;

        Bullet bullet2 = GetBullet(1);
        Vector3 bullet2Pos = transform.position;
        bullet2Pos += bulletShootPosList[(int)EShootPos.Right];
        bullet2.transform.position = bullet2Pos;

        yield break;
    }

    private IEnumerator IShootingPattern7()
    {
        Bullet bullet1 = GetBullet(2);
        Vector3 bullet1Pos = transform.position;
        bullet1Pos += bulletShootPosList[(int)EShootPos.Left];
        bullet1.transform.position = bullet1Pos;

        Bullet bullet2 = GetBullet(3);
        Vector3 bullet2Pos = transform.position;
        bullet2Pos += bulletShootPosList[(int)EShootPos.Center];
        bullet2.transform.position = bullet2Pos;

        Bullet bullet3 = GetBullet(2);
        Vector3 bullet3Pos = transform.position;
        bullet3Pos += bulletShootPosList[(int)EShootPos.Right];
        bullet3.transform.position = bullet3Pos;

        yield break;
    }
    #endregion

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
    public void AddPet(int petCount)
    {
        Instantiate(petList[petCount - 1], transform.position, Quaternion.identity);
    }

    public void OnDamaged(float dmg)
    {
        Hp -= dmg;
    }

    private void OnDie()
    {
        print("죽었어임마");
    }
}
