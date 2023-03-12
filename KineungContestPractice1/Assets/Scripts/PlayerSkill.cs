using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public enum ESkillType
{
    GuideShoot,
}

public class PlayerSkill : MonoBehaviour
{
    public List<bool> skillCoolDownList = new List<bool>();

    private Player player;

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

    private void SkillKeyInput()
    {
        if (Input.GetKeyDown(KeyCode.X) && skillCoolDownList[(int)ESkillType.GuideShoot] == false)
        {
            Enemy enemy = FindObjectOfType<Enemy>();
            if (enemy == null) return;

            target = enemy.transform;

            //if (FindObjectOfType<Enemy>() == null) return;
            //target = FindObjectOfType<Enemy>().transform;

            ChaseShoot();
            StartCoroutine(ISkillCoolDown(ESkillType.GuideShoot));
        }
    }

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
}
