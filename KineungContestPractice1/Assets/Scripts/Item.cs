using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EItemType
{
    LevelUp,
    Healing,
    AddPet,
}
public class Item : MonoBehaviour
{
    public EItemType type;

    private float spd = 2f;
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
                Player.Instance.Level += 1;
                break;
            case EItemType.Healing:
                Player.Instance.Hp += 1;
                break;
            case EItemType.AddPet:
                Player.Instance.PetCount += 1;
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
}
