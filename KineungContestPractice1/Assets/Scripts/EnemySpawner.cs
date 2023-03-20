using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.UIElements;

public enum EEnemySpawnPosType
{
    LeftMiddle,
    LeftTop,
    CenterLeft,
    Center,
    CenterRight,
    RightMiddle,
    RightTop,
}
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
        StartCoroutine(SpawnPatterns());
        SpawnBoss(1);
    }

    private IEnumerator SpawnPatterns()
    {
        yield return new WaitForSeconds(5f);

        GameManager.Instance.stage = 1;
        GetEnemy1((int)EEnemySpawnPosType.CenterLeft);
        GetEnemy2((int)EEnemySpawnPosType.CenterRight);
        yield return new WaitForSeconds(2f);
        GetEnemy1((int)EEnemySpawnPosType.LeftTop);
        GetEnemy3((int)EEnemySpawnPosType.Center);
        yield return new WaitForSeconds(3f);
        GetEnemy2((int)EEnemySpawnPosType.LeftMiddle);
        GetEnemy1((int)EEnemySpawnPosType.RightMiddle);
        yield return new WaitForSeconds(2f);
        GetEnemy3((int)EEnemySpawnPosType.Center);
        GetEnemy1((int)EEnemySpawnPosType.LeftMiddle);
        yield return new WaitForSeconds(3f);
        GetEnemy3((int)EEnemySpawnPosType.RightMiddle);
        GetEnemy2((int)EEnemySpawnPosType.CenterRight);
        yield return new WaitForSeconds(3f);
        GetEnemy1((int)EEnemySpawnPosType.LeftTop);
        GetEnemy2((int)EEnemySpawnPosType.Center);
        yield return new WaitForSeconds(2f);
        GetEnemy3((int)EEnemySpawnPosType.RightTop);
        GetEnemy2((int)EEnemySpawnPosType.LeftMiddle);
        yield return new WaitForSeconds(3f);
        GetEnemy1((int)EEnemySpawnPosType.CenterLeft);
        GetEnemy1((int)EEnemySpawnPosType.CenterRight);
        yield return new WaitForSeconds(3f);
        GetEnemy2((int)EEnemySpawnPosType.LeftTop);
        GetEnemy3((int)EEnemySpawnPosType.Center);
        yield return new WaitForSeconds(2f);
        GetEnemy1((int)EEnemySpawnPosType.RightMiddle);
        GetEnemy2((int)EEnemySpawnPosType.CenterLeft);
        yield return new WaitForSeconds(3f);
        GetEnemy1((int)EEnemySpawnPosType.CenterRight);
        GetEnemy3((int)EEnemySpawnPosType.Center);
        yield return new WaitForSeconds(3f);
        GetEnemy2((int)EEnemySpawnPosType.LeftTop);
        GetEnemy1((int)EEnemySpawnPosType.LeftMiddle);
        yield return new WaitForSeconds(4f);
        GetEnemy2((int)EEnemySpawnPosType.CenterLeft);
        GetEnemy3((int)EEnemySpawnPosType.CenterLeft);
        yield return new WaitForSeconds(1f);
        StartCoroutine(SpawnBoss(1));
    }

    private Enemy GetEnemy(int enemyNum, int posNum,Vector3 rotate = default)
    {
        Enemy enemy = Instantiate(enemyList[enemyNum], enemySpawnPos[posNum].position, Quaternion.identity);

        
        if(posNum == 0 || posNum == 1)
        {
            rotate = new Vector3(0,0, 90);
        }
        else if(posNum == 5 || posNum == 6)
        {
            rotate = new Vector3(0,0, -90);
        }
        else
        {
            rotate = default;
        }

        enemy.transform.rotation = Quaternion.Euler(rotate);

        return enemy;
    }

    private IEnumerator SpawnBoss(int bossNum)
    {

        Instantiate(bossList[bossNum - 1], enemySpawnPos[3].position + new Vector3(0, 2, 0), Quaternion.identity);
        isBossSpawned = true;
        yield return null;
    }

    private void GetEnemy1(int posNum, Vector3 rotate = default)
    {
        GetEnemy(0, posNum, rotate);
    }

    private void GetEnemy2(int posNum, Vector3 rotate = default)
    {
        GetEnemy(1, posNum, rotate);
    }

    private void GetEnemy3(int posNum, Vector3 rotate = default)
    {
        GetEnemy(2, posNum, rotate);
    }

    private void GetEnemy4(int posNum, Vector3 rotate = default)
    {
        GetEnemy(3, posNum, rotate);
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
