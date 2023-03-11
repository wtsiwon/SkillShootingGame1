using Microsoft.Unity.VisualStudio.Editor;
using System.Collections;
using System.Collections.Generic;
using Unity.Profiling;
using UnityEngine;
using UnityEngine.UI;
using Image = UnityEngine.UI.Image;
using TMPro;

public class GameManager : Singleton<GameManager>
{
    public Image[] hpIcons;

    public TextMeshProUGUI scoreText;

    public GameObject destroyEffect;

    public List<Item> itemList = new List<Item>();

    private Player player;

    [Header("카메라Shake 관련 변수")]
    public Camera cam;

    public float shakeTime;
    public float shakeInterval;
    public float shakeRange;

    private int score;

    [HideInInspector]
    public float destroyDistance = 14f;

    public int currentStageNum;

    public int Score
    {
        get => score;
        set
        {
            score = value;
            scoreText.text = $"{score}";
        }
    }

    public int stage;

    private void Awake()
    {
    }

    void Start()
    {
        InitializeGame();
    }

    void Update()
    {

    }

    private void InitializeGame()
    {
        player = Player.Instance;
        Score = 0;

        StartCoroutine(CameraShake(shakeTime, shakeRange));
    }

    public void UpdatePlayerHpIcon(int hp)
    {
        for (int i = 0; i < hpIcons.Length; i++)
        {
            hpIcons[i].gameObject.SetActive(false);
            if (i <= hp - 1)
            {
                hpIcons[i].gameObject.SetActive(true);
            }
        }
    }

    public void GetDestroyEffect(Vector3 pos, float scale = 7, bool isRandRotate = true)
    {
        float randRotate;
        if (isRandRotate)
        {
            randRotate = Random.Range(0, 360);
        }
        else
        {
            randRotate = 0;
        }

        GameObject obj = Instantiate(destroyEffect, pos, Quaternion.identity);
        obj.transform.rotation = Quaternion.Euler(0, 0, randRotate);
        obj.transform.localScale = new Vector3(scale, scale, 1);
        Destroy(obj, 0.5f);
    }

    public void SpawnRandomItem(Vector3 pos)
    {
        int randNum;
        if (player.PetCount != player.maxPetCount)
        {
            randNum = Random.Range(0, itemList.Count);
        }
        else
        {
            randNum = Random.Range(0, itemList.Count - 1);
        }
        Instantiate(itemList[randNum], pos, Quaternion.identity);
    }

    #region 카메라 Shake
    /// <summary>
    /// 
    /// </summary>
    /// <param name="shakeTime">흔드는 시간</param>
    /// <param name="range">얼마나 크게 움직일 것인가</param>
    /// <param name="ifMode">시간이 아니라 특정조건에 만족해야 할때</param>
    public IEnumerator CameraShake(float shakeTime, float range, bool ifMode = false)
    {
        Vector3 defaultCamPosition = cam.transform.localPosition;

        if (ifMode == false)
        {
            float time = 0;

            while (time < shakeTime)
            {
                StartCoroutine(ShakePosition(range));

                time += Time.deltaTime;
                yield return null;
            }
        }
        else
        {
            bool isDone = false;
            while (isDone == false)
            {
                ShakePosition(range);
                isDone = true;
                yield break;
            }
        }

        cam.transform.localPosition = defaultCamPosition;
    }

    private IEnumerator ShakePosition(float range)
    {
        Vector3 shakePosition = Random.insideUnitCircle * range;
        
        shakePosition.z = cam.transform.localPosition.z;

        cam.transform.localPosition = shakePosition;
        print(cam.transform.localPosition);

        ShakeIntervalMove(shakePosition, 0.1f);
        yield return new WaitForSeconds(0.1f);
    }

    private void ShakeIntervalMove(Vector3 shakePosition, float shakeInterval)
    {
        print("CameraShake");
        float time = 0;
        while (time < shakeInterval)
        {
            cam.transform.localPosition = Vector3.Lerp(cam.transform.localPosition, shakePosition, time / shakeInterval);
            time += Time.deltaTime;
        }
    }

    #endregion

    private void GameOver()
    {

    }
}
