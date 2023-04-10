using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjPool : Singleton<ObjPool>
{
    public Bullet[] originObj;

    private Dictionary<int, Queue<Bullet>> pool = new Dictionary<int, Queue<Bullet>>();

    public Bullet GetBullet(Vector3 pos, int poolType, Quaternion rot)
    {
        Bullet bullet = null;


        if(pool.ContainsKey(poolType) == false)
        {
            pool.Add(poolType, new Queue<Bullet>());
        }

        Queue<Bullet> queue = pool[poolType];

        Bullet origin = originObj[poolType - 1];

        if (pool[poolType].Count > 0)
        {
            bullet = queue.Dequeue();
        }
        else
        {
            bullet = Instantiate(origin);
        }

        bullet.transform.position = pos;
        bullet.transform.rotation = rot;
        bullet.gameObject.SetActive(true);

        return bullet;
    }

    public void Return(int poolType, Bullet bullet)
    {
        bullet.gameObject.SetActive(false);
        pool[poolType].Enqueue(bullet);
    }

}
