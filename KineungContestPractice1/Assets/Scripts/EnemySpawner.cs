using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.UIElements;

public class EnemySpawner : MonoBehaviour
{
    private static EnemySpawner instance;
    public static EnemySpawner Instance
    {
        get => instance;
    }

    public bool isEnemySpawn;

    public bool isBossSpawned;

    public List<Enemy> enemyList = new List<Enemy>();

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        
    }

    private IEnumerator ISpawn()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.01f);
            if (isEnemySpawn)
            {
                yield return new WaitForSeconds(3f);
            }
        }
    }

    private void BossSpawn()
    {

    }
}
