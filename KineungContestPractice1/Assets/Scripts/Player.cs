using System.Collections;
using System.Collections.Generic;
using UnityEditor.U2D.Sprites;
using UnityEngine;
using UnityEngine.UIElements;

public class Player : MonoBehaviour
{
    public float moveSpd;

    public float atkSpd;

    public float bulletSpd;

    public float atkDmg;

    [Space(10f)]
    public bool isShoot;

    [Space(10f)]
    public int level;

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
        if (Input.GetKey(KeyCode.J))
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
                Bullet bullet1 = Instantiate(bullet);
                bullet1.SetBullet(transform.position, Vector3.up, bulletSpd, level);
                yield return new WaitForSeconds(atkSpd);
            }

        }
    }

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

    public void OnDamaged(float dmg)
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
