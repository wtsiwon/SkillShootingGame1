using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EItemType
{
    LevelUp,//���� ��
    Healing,//��
    Meteor,//���׿�
    Invincibility,//����
    AddPet,//�� �߰�
    FuelSupply,//���� ����
}
public class Item : MonoBehaviour
{
    public EItemType type;

    private float spd = 2f;

    [SerializeField]
    [Tooltip("��ź ������")]
    private int bombDmg;

    [Tooltip("�����ð�")]
    [SerializeField]
    private float invicibilityTime;

    [SerializeField]
    [Tooltip("���� ������")]
    private float fuelSupplyAmount;

    [SerializeField]
    [Tooltip("���׿� GameObject")]
    private Meteor meteor;

    private Coroutine Iinvicibility;

    private void Start()
    {
        SetItem();
    }

    private void Update()
    {
        transform.Translate(Vector3.down * spd * Time.deltaTime);
    }

    private void SetItem()
    {
        switch (type)
        {
            case EItemType.LevelUp:
                if (Player.Instance.Level == Player.Instance.maxLevel)
                {
                    GameManager.Instance.Score += 10000;
                }
                else
                {
                    Player.Instance.Level += 1;
                }
                break;
            case EItemType.Healing:
                if (Player.Instance.maxHp == Player.Instance.Hp)
                {
                    GameManager.Instance.Score += 10000;
                }
                else
                {
                    Player.Instance.Hp += 20;
                }
                break;
            case EItemType.Meteor:
                //���׿�
                SpawnMeteor();
                break;
            case EItemType.AddPet:
                if (Player.Instance.PetCount == Player.Instance.maxPetCount)
                {
                    GameManager.Instance.Score += 10000;
                }
                else
                {
                    Player.Instance.PetCount += 1;
                }

                break;
            case EItemType.Invincibility:
                if (Player.Instance.IsInvicibility == true)
                {
                    StopCoroutine(nameof(IInvicibility));
                }

                StartCoroutine(nameof(IInvicibility));
                break;
            case EItemType.FuelSupply:
                FuelSupply(fuelSupplyAmount);
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Player>(out Player player))
        {
            SetItem();
            Destroy(gameObject);
        }
    }

    private void SpawnMeteor()
    {
        Instantiate(meteor);
    }

    private IEnumerator IInvicibility()
    {
        Player.Instance.IsInvicibility = true;
        yield return new WaitForSeconds(invicibilityTime);
        Player.Instance.IsInvicibility = false;
    }

    private void FuelSupply(float amount)
    {
        Player.Instance.Fuel += amount;
    }
}
