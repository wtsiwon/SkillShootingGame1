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
    [SerializeField]
    private BoxCollider2D collider;

    public Slider hpbar;
    public Slider fuelbar;

    [Header("Texts")]
    [Space(10f)]
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI highestScoreText;
    public Text cantUseSkillText;
    public Text noEnemyText;
    public float textFadeOutTime;


    public GameObject destroyEffect;
    [Space(5f)]
    public List<Item> itemList = new List<Item>();

    private Player player;

    [Header("ī�޶�Shake ���� ����")]
    [Space(10f)]
    public Camera cam;

    public float shakeTime;
    public float shakeInterval;
    public float shakeRange;


    [HideInInspector]
    public float destroyDistance = 14f;

    [Space(10f)]
    public int currentStageNum;

    private int highestScore;

    public int HighestScore
    {
        get
        {
            if (highestScore <= score)
            {
                PlayerPrefs.SetInt("HighestScore", score);
            }
            else
            {
                highestScore = PlayerPrefs.GetInt("HighestScore", 0);
            }

            return highestScore;
        }

        set
        {
            highestScoreText.text = highestScore.ToString();
        }
    }

    private int score;

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
        StartCoroutine(IUpdate());
    }

    void Update()
    {

    }

    private IEnumerator IUpdate()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.5f);
            print(fuelbar.value);
        }
    }

    private void InitializeGame()
    {
        player = Player.Instance;
        Score = 0;
    }

    public void UpdatePlayerHpBar(float amount)
    {
        hpbar.value = amount;
    }

    public void UpdatePlayerFuelBar(float amount)
    {
        fuelbar.value = amount;
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

    #region ī�޶� Shake
    /// <summary>
    /// 
    /// </summary>
    /// <param name="shakeTime">���� �ð�</param>
    /// <param name="range">�󸶳� ũ�� ������ ���ΰ�</param>
    /// <param name="ifMode">�ð��� �ƴ϶� Ư�����ǿ� �����ؾ� �Ҷ�</param>
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

    #region Activing Text
    public void CantUseSkillText()
    {
        Text text = Instantiate(cantUseSkillText, Vector3.zero, Quaternion.identity);
        StartCoroutine(ITextFadeOut(text));
        
    }

    public void NoEnemyText()
    {
        Text text = Instantiate(noEnemyText, Vector3.zero, Quaternion.identity);
        StartCoroutine(ITextFadeOut(text));
    }

    private IEnumerator ITextFadeOut(Text text)
    {
        float time = 0;
        
        while(time < textFadeOutTime)
        {
            yield return new WaitForSeconds(0.05f);
            time += 0.05f;
            text.color = Color.Lerp(text.color, new Color(text.color.r, text.color.g, text.color.b, 0), time/ textFadeOutTime);
        }

        Destroy(text);
    }
    #endregion

    private void GameOver()
    {

    }
}
