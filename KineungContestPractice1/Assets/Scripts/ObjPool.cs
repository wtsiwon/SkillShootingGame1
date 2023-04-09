using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjPool : Singleton<ObjPool>
{
    public Bullet[] originObj;

    private Dictionary<int, Queue<Bullet>> pool = new Dictionary<int, Queue<Bullet>>();

    public Bullet Get(Vector3 pos, int poolType)
    {
        Bullet bullet = null;


        if(pool.ContainsKey(poolType) == false)
        {
            pool.Add(poolType, new Queue<Bullet>());
        }



        if (pool[poolType].Count > 0)
        {
            bullet = pool[poolType].Dequeue();
        }
        else
        {
            bullet = Instantiate(bullet, pos, Quaternion.identity);
        }



        return bullet;
    }

}
