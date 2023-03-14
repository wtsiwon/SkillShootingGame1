using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public enum ESkillType
{
    BezierShoot,
    Repair,
    Bomb,
}

public class PlayerSkill : MonoBehaviour
{
    [Tooltip("스킬 쿨타임들")]
    public List<bool> skillCoolDownList = new List<bool>();

    [SerializeField]
    private BoxCollider2D collider;

    private Player player;


    [Tooltip("폭탄 스킬 데미지")]
    public int bombDmg;

    [Tooltip("얼마나 수리 해줄것인가")]
    public int repairAmount;

    public int shootCount;

    public BezierBullet bezierBullet;

    [SerializeField]
    [Space(10f)]
    private Transform target;

    private float startDistance = 6f;
    private float endDistance = 3f;

    #region SkillUI
    [SerializeField]
    [Space(10f)]
    private List<Image> skillIconList = new List<Image>();

    [SerializeField]
    [Space(10f)]
    private List<Text> skillCoolTimeTextList = new List<Text>();
    #endregion

    [SerializeField]
    [Space(10f)]
    private List<float> skillCoolTimeList = new List<float>();

    private void Start()
    {
        player = Player.Instance;
    }

    private void Update()
    {
        SkillKeyInput();
    }

    #region SkillKeyInput
    private void SkillKeyInput()
    {
        BezierSkillInput();
        BombSkillInput();
        RepairSkillInput();
    }

    private void BezierSkillInput()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            if(skillCoolDownList[(int)ESkillType.BezierShoot] == true)
            {
                GameManager.Instance.CantUseSkillText();
            }

            Enemy enemy = FindObjectOfType<Enemy>();
            if (enemy == null) return;

            target = enemy.transform;

            ChaseShoot();
            StartCoroutine(ISkillCoolDown(ESkillType.BezierShoot));
        }
    }

    private void BombSkillInput()
    {
        if(Input.GetKeyDown(KeyCode.C))
        {
            if(skillCoolDownList[(int)ESkillType.Bomb] == true)
            {
                GameManager.Instance.CantUseSkillText();
            }

            Boom(bombDmg);
            StartCoroutine(ISkillCoolDown(ESkillType.Bomb));
        }
    }

    private void RepairSkillInput()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            if (skillCoolDownList[(int)ESkillType.Repair] == true)
            {
                GameManager.Instance.CantUseSkillText();
            }

            Repair(repairAmount);
            StartCoroutine(ISkillCoolDown(ESkillType.Repair));
        }
    }
    #endregion

    private IEnumerator ISkillCoolDown(ESkillType type)
    {
        int index = (int)type;

        skillCoolDownList[index] = true;

        float timer = 0;
        skillCoolTimeTextList[index].gameObject.SetActive(true);
        

        while (timer < skillCoolTimeList[index])
        {
            skillIconList[index].fillAmount = timer / skillCoolTimeList[index];

            skillCoolTimeTextList[index].text = $"{(int)(skillCoolTimeList[index] - timer)}";

            timer += Time.deltaTime;
            yield return null;
        }
        skillCoolDownList[index] = false;

        skillCoolTimeTextList[index].gameObject.SetActive(false);
    }

    private void ChaseShoot()
    {
        StartCoroutine(IGuideShoot1(shootCount));
    }

    private IEnumerator IGuideShoot1(int count)
    {
        if (target == null) yield break;

        for (int i = 0; i < count / 2; i++)
        {
            for (int j = 0; j < 2; j++)
            {
                if (target == null) yield return null;
                BezierBullet bullet1 = Instantiate(bezierBullet);
                bullet1.SetBullet(transform.position, player.bulletSpd / 10, player.atkDmg, target, startDistance, endDistance, false);
            }
            yield return new WaitForSeconds(0.05f);
        }
    }

    private void Boom(int dmg)
    {
        List<Enemy> enemies = new List<Enemy>();
        List<Bullet> bullets = new List<Bullet>();
        Collider2D[] objs = Physics2D.OverlapBoxAll(Vector2.zero, collider.size, 0);

        for (int i = 0; i < objs.Length; i++)
        {
            if (TryGetComponent<Enemy>(out Enemy enemy))
            {
                enemies.Add(enemy);
            }
            else if(TryGetComponent<Bullet>(out Bullet bullet))
            {
                bullets.Add(bullet);
            }
        }

        if(enemies.Count == 0 && bullets.Count == 0)
        {
            return;
        }

        for (int i = 0; i < enemies.Count; i++)
        {
            enemies[i].OnDamaged(dmg);
        }

        for (int i = 0; i < bullets.Count; i++)
        {
            GameManager.Instance.GetDestroyEffect(bullets[i].transform.position);
            Destroy(bullets[i].gameObject);
        }
    }

    private void Repair(int amount)
    {
        player.Hp += amount;
    }
}
