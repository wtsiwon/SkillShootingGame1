using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : Singleton<GameManager>
{
    public Canvas canvas;
    public Bullet bullet;

    [Header("UI")]
    public Slider hpbar;
    public Slider fuelbar;

    [Header("Texts")]
    [Space(10f)]
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI highestScoreText;
    public TextMeshProUGUI timerText;
    public Text cantUseSkillText;
    public Text noEnemyText;
    public Text healingText;
    public GameObject healingEffect;
    public float textFadeOutTime;

    public bool isGameStart;

    public GameObject destroyEffect;
    [Space(5f)]
    public List<Item> itemList = new List<Item>();

    [Tooltip("10을 기준으로 설정")]
    public List<float> itemSpawnPercentageList = new List<float>();

    private Player player;

    [Header("카메라Shake 관련 변수")]
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

    [SerializeField]
    [Tooltip("초당 점수 증가량")]
    private int scoreIncreasePerSecond;

    private int score;

    public int Score
    {
        get => score;
        set
        {
            score = value;
            if (score >= HighestScore)
            {
                highestScoreText.text = $"{score}";
            }
            scoreText.text = $"{score}";
        }
    }

    public int stage;

    private float timer;

    public float Timer
    {
        get => timer;
        set
        {
            timer = value;
            timerText.text = $"{Mathf.Round(Timer * 10) * 0.1f}s";//소수점 첫째자리까지 표시
        }
    }


    private void Awake()
    {

    }

    void Start()
    {
        InitializeGame();
        StartCoroutine(nameof(IUpdate));
        StartCoroutine(nameof(IAddScore));
        StartCoroutine(nameof(ITimer));
    }

    void Update()
    {
    }

    private IEnumerator ITimer()
    {
        while (true)
        {
            yield return null;
            if (isGameStart == true)
            {
                Timer += Time.deltaTime;
            }
        }
    }

    private IEnumerator IAddScore()
    {
        while (true)
        {
            yield return null;
            if (isGameStart == true)
            {
                yield return new WaitForSeconds(0.1f);
                Score += scoreIncreasePerSecond / 10;
            }
        }
    }

    private IEnumerator IUpdate()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.5f);
        }
    }

    private void InitializeGame()
    {
        player = Player.Instance;
        Score = 0;
        isGameStart = true;

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
        int itemNum = 0;
        int randNum = Random.Range(0, 100);

        if (itemSpawnPercentageList[(int)EItemType.LevelUp] < randNum * 10)
        {

        }
        else if (itemSpawnPercentageList[(int)EItemType.Healing] < randNum)
        {

        }
        else if (itemSpawnPercentageList[(int)EItemType.Bomb] < randNum)
        {

        }
        else if (itemSpawnPercentageList[(int)EItemType.Invincibility] < randNum)
        {

        }




        //Instantiate(itemList[], pos, Quaternion.identity);
    }

    public static bool GetThisChanceResult(float chance)
    {
        if(chance < 0.0000001f)
        {
            chance = 0.0000001f;
        }

        bool success = false;
        int randAccuracy = 1000000;

        float randHitRange = chance * randAccuracy;
        int rand = Random.Range(1, randAccuracy + 1);
        if(rand < randHitRange)
        {
            success = true;
        }
        return success;
    }


    #region 카메라 Shake

    public void CameraShake(float shakeTime, float range, bool ifMode = false)
    {
        StartCoroutine(ICameraShake(shakeTime, range, ifMode));
    }

    /// <summary>
    /// 카메라 흔드는 코루틴
    /// </summary>
    /// <param name="shakeTime">흔드는 시간</param>
    /// <param name="range">얼마나 크게 움직일 것인가</param>
    /// <param name="ifMode">시간이 아니라 특정조건에 만족해야 할때</param>
    private IEnumerator ICameraShake(float shakeTime, float range, bool ifMode = false)
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
        Text text = Instantiate(cantUseSkillText, canvas.transform.position, Quaternion.identity, canvas.transform);
        StartCoroutine(ITextFadeOut(text, textFadeOutTime));
    }

    public void NoEnemyText()
    {
        Text text = Instantiate(noEnemyText, canvas.transform.position, Quaternion.identity, canvas.transform);
        StartCoroutine(ITextFadeOut(text, textFadeOutTime));
    }

    public void HealingText(int amount)
    {
        Text text = Instantiate(healingText, hpbar.transform.position, Quaternion.identity, canvas.transform);
        text.transform.position = new Vector3(text.transform.position.x, text.transform.position.y - 330, 0);

        text.text = $"+{amount}Hp!";

        StartCoroutine(ITextFadeOut(text, textFadeOutTime * 2));
    }

    public void HealingEffect(int count, Vector3 pos)
    {
        for (int i = 0; i < count; i++)
        {
            Instantiate(healingEffect, pos, Quaternion.identity);
        }
    }

    private IEnumerator ITextFadeOut(Text text, float fadeOutTime)
    {
        float time = 0;

        while (time < fadeOutTime)
        {
            yield return new WaitForSeconds(0.05f);
            time += 0.05f;
            text.color = Color.Lerp(text.color, new Color(text.color.r, text.color.g, text.color.b, 0), time / textFadeOutTime);
        }

        Destroy(text.gameObject);
    }
    #endregion

    private void GameOver()
    {

    }
}
