using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy3 : Enemy
{
    protected override void Attack()
    {
        RandShot(8);
    }

    private void RandShot(int count)
    {
        
    }
}
