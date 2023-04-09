using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
public enum ESkillType
{
    BezierShoot,
    Repair,
    Bomb,
    Revolution,
}

public class PlayerSkill : MonoBehaviour
{
    [Tooltip("��ų ��Ÿ���� ���� �ֳ� ������, ����, ��ź��")]
    public List<bool> skillCoolDownList = new List<bool>();

    [SerializeField]
    [Space(10f)]
    private List<float> skillCoolTimeList = new List<float>();


    private Player player;

    [Header("��ź ������")]
    [SerializeField]
    private BoxCollider2D bombCollider;

    [Tooltip("��ź ��ų ������")]
    public int bombDmg;

    [Tooltip("�󸶳� ���� ���ٰ��ΰ�")]
    public int repairAmount;

    #region ������ ���� ������
    [Header("������ ���ݺ�����")]
    public int shootCount;
    public BezierBullet bezierBullet;

    [SerializeField]
    [Space(10f)]
    private Transform target;

    private float startDistance = 6f;
    private float endDistance = 3f;

    #endregion

    #region ���� ��ų ���� ����
    [Header("������ų ���� ������")]
    [SerializeField]
    private RevolutionObj revolutionObj;

    [SerializeField]
    private float revolutionDuration;

    [SerializeField]
    private float revolutionSpd;
    #endregion

    private bool revolutionDone = true;
    #region SkillUI
    [Header("SkillUI")]
    [SerializeField]
    [Space(10f)]
    private List<Image> skillIconList = new List<Image>();

    [SerializeField]
    [Space(10f)]
    private List<Text> skillCoolTimeTextList = new List<Text>();
    #endregion

    private void Start()
    {
        player = Player.Instance;
        SetSkill();
    }

    private void Update()
    {
        SkillKeyInput();
    }

    private void SetSkill()
    {
        revolutionObj.rotateSpd = revolutionSpd;
    }

    #region SkillKeyInput
    private void SkillKeyInput()
    {
        BezierSkillInput();
        BombSkillInput();
        RepairSkillInput();
        RevolutionSkillInput();
    }

    private void BezierSkillInput()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            if (skillCoolDownList[(int)ESkillType.BezierShoot] == true)
            {
                GameManager.Instance.CantUseSkillText();
                return;
            }

            Enemy enemy = FindObjectOfType<Enemy>();
            if (enemy == null)
            {
                GameManager.Instance.NoEnemyText();
                return;
            }

            target = enemy.transform;

            ChaseShoot();
            StartCoroutine(ISkillCoolDown(ESkillType.BezierShoot));
        }
    }

    private void BombSkillInput()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            if (skillCoolDownList[(int)ESkillType.Bomb] == true)
            {
                GameManager.Instance.CantUseSkillText();
                return;
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
                return;
            }

            Repair(repairAmount);
            GameManager.Instance.HealingText(repairAmount);
            StartCoroutine(ISkillCoolDown(ESkillType.Repair));
        }
    }

    private void RevolutionSkillInput()
    {
        if (Input.GetKeyDown(KeyCode.LeftAlt))
        {
            if (skillCoolDownList[(int)ESkillType.Revolution] == true || revolutionDone == false)
            {
                GameManager.Instance.CantUseSkillText();
                return;
            }

            Revolution();
            //��ų�� �� ������ ��Ÿ�� �־���
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
            yield return new WaitForSeconds(3f / 100f);
        }
    }

    private void Boom(int dmg)
    {
        List<Enemy> enemies = new List<Enemy>();
        List<Bullet> bullets = new List<Bullet>();
        Collider2D[] objs = Physics2D.OverlapBoxAll(Vector2.zero, bombCollider.size, 0);

        //enemies = objs.Select(obj => GetComponent<Enemy>()).Where(enemy => enemy != null).ToList();
        //bullets = objs.Select(obj => GetComponent<Bullet>()).Where(bullet => bullet != null).ToList();

        #region LINQ�� �ٲٱ� �� �ڵ�
        for (int i = 0; i < objs.Length; i++)
        {
            if (objs[i].TryGetComponent(out Enemy enemy))
            {
                enemies.Add(enemy);
            }
            else if (objs[i].TryGetComponent(out Bullet bullet))
            {
                bullets.Add(bullet);
            }
        }
        #endregion

        if (enemies.Count == 0 && bullets.Count == 0)
        {
            print("�ƹ��� �����Ӹ�");
            return;
        }

        if (enemies.Count > 0)
        {
            for (int i = 0; i < enemies.Count; i++)
            {
                enemies[i].OnDamaged(dmg);
            }
        }

        if (bullets.Count > 0)
        {
            for (int i = 0; i < bullets.Count; i++)
            {
                GameManager.Instance.GetDestroyEffect(bullets[i].transform.position);
                Destroy(bullets[i].gameObject);
            }
        }
        GameManager.Instance.GetDestroyEffect(Vector3.zero, 100);
        GameManager.Instance.FlashEffect(0.66f);
    }

    private void Repair(int amount)
    {
        player.Hp += amount;
        GameManager.Instance.HealingEffect(3, transform.position);
    }

    /// <summary>
    /// ����
    /// </summary>
    /// <param name="spd">���� �ӵ�</param>
    private void Revolution()
    {
        StartCoroutine(IRevolution());
    }

    private IEnumerator IRevolution()
    {
        revolutionDone = false;
        revolutionObj.gameObject.SetActive(true);
        yield return new WaitForSeconds(revolutionDuration);
        revolutionObj.gameObject.SetActive(false);
        StartCoroutine(ISkillCoolDown(ESkillType.Revolution));
        revolutionDone = true;
    }
}