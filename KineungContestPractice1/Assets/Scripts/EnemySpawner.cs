using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.UIElements;

public class EnemySpawner : Singleton<EnemySpawner>
{
    public bool isEnemySpawn;

    public bool isBossSpawned;

    public List<Enemy> enemyList = new List<Enemy>();

    [Space(10f)]
    public List<Boss> bossList = new List<Boss>();

    [Space(10f)]
    public List<Transform> enemySpawnPos = new List<Transform>();

    private WaitForSeconds waitTime = new WaitForSeconds(3);

    private void Awake()
    {

    }
    private void Start()
    {
        SpawnPatterns();
    }

    private void SpawnPatterns()
    {
        SpawnBoss(1);
    }

    private Enemy GetEnemy(int enemyNum, int posNum,Vector3 rotate = default)
    {
        Enemy enemy = Instantiate(enemyList[enemyNum], enemySpawnPos[posNum].position, Quaternion.identity);
        enemy.transform.rotation = Quaternion.Euler(rotate);
        return enemy;
    }

    private void SpawnBoss(int bossNum)
    {
        Instantiate(bossList[bossNum - 1], enemySpawnPos[3].position + new Vector3(0, 2, 0), Quaternion.identity);
        isBossSpawned = true;
    }

    #region 적 나오는 패턴들

    private IEnumerator IEnemySpawnPattern1()
    {
        GetEnemy(0, 0, new Vector3(0, 0, 45));
        yield return new WaitForSeconds(0.1f);
        GetEnemy(1, 6, new Vector3(0, 0, -45));
        yield return new WaitForSeconds(0.2f);
        GetEnemy(1, 1, new Vector3(0, 0, 45));
        yield return new WaitForSeconds(0.1f);
        GetEnemy(0, 5, new Vector3(0, 0, -45));
    }

    private IEnumerator IEnemySpawnPattern2()
    {
        GetEnemy(3, 0);
        GetEnemy(3, 6);
        yield return new WaitForSeconds(0.2f);
        GetEnemy(2, 1);
        GetEnemy(2, 5);
        yield return new WaitForSeconds(0.2f);
        GetEnemy(1, 2);
        GetEnemy(1, 4);
        yield return new WaitForSeconds(0.2f);
        GetEnemy(0, 3);
    }

    private IEnumerator IEnemySpawnPattern3()
    {
        GetEnemy(0, 3);
        yield return new WaitForSeconds(0.2f);
        GetEnemy(1, 2);
        GetEnemy(1, 4);
        yield return new WaitForSeconds(0.2f);
        GetEnemy(2, 1);
        GetEnemy(2, 5);
        yield return new WaitForSeconds(0.2f);
        GetEnemy(3, 0);
        GetEnemy(3, 6);
    }

    private IEnumerator IEnemySpawnPattern4()
    {
        float enemyMoveSpd = 10f;

        Enemy enemy1 = GetEnemy(0, 1);
        enemy1.moveSpd = enemyMoveSpd;

        Enemy enemy2 = GetEnemy(0, 5);
        enemy2.moveSpd = enemyMoveSpd;

        yield return new WaitForSeconds(0.5f);

        Enemy enemy3 = GetEnemy(1, 2);
        enemy3.moveSpd = enemyMoveSpd;

        Enemy enemy4 = GetEnemy(1, 4);
        enemy4.moveSpd = enemyMoveSpd;
    }

    private IEnumerator IEnemySpawnPattern5()
    {
        float enemyMoveSpd = 10f;

        Enemy enemy1 = GetEnemy(2, 1);
        enemy1.moveSpd = enemyMoveSpd;

        Enemy enemy2 = GetEnemy(2, 5);
        yield return new WaitForSeconds(0.5f);

        Enemy enemy3 = GetEnemy(3, 2);
        enemy3.moveSpd = enemyMoveSpd;

        Enemy enemy4 = GetEnemy(3, 4);
        enemy4.moveSpd = enemyMoveSpd;
    }

    private IEnumerator IEnemySpawnPattern6()
    {
        GetEnemy(0, 2);
        GetEnemy(0, 4);
        yield return new WaitForSeconds(0.2f);
        GetEnemy(1, 3);
    }

    private IEnumerator IEnemySpawnPattern7()
    {
        GetEnemy(2, 1);
        GetEnemy(2, 5);
        yield return new WaitForSeconds(0.4f);
        GetEnemy(2, 1);
        GetEnemy(2, 5);
    }

    #endregion
}
