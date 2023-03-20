using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy4 : Enemy
{
    [SerializeField]
    private Meteor meteor;
    protected override void Attack()
    {
        StartCoroutine(ISpawnMeteor());
    }

    private IEnumerator ISpawnMeteor()
    {
        Instantiate(meteor);
        yield return null;
    }

}
