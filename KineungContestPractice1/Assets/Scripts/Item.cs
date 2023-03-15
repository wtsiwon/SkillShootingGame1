using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EItemType
{
    LevelUp,
    Healing,
    Bomb,
    Invincibility,
    FuelSupply,
    AddPet,
}
public class Item : MonoBehaviour
{
    public EItemType type;

    private float spd = 2f;

    [SerializeField]
    [Tooltip("폭탄 데미지")]
    private int bombDmg;

    [SerializeField]
    [Tooltip("무적시간")]
    private float invicibilityTime;

    [SerializeField]
    [Tooltip("연료 충전량")]
    private float fuelSupplyAmount;

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
                Player.Instance.Hp += 20;
                break;
            case EItemType.AddPet:
                Player.Instance.PetCount += 1;
                break;
            case EItemType.Invincibility:
                StartCoroutine(IInvicibility(invicibilityTime));
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

    private IEnumerator IInvicibility(float invicibilityTime)
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
