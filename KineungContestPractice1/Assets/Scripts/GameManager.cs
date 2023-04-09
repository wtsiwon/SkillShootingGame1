using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    public Canvas canvas;

    [SerializeField]
    private Image whiteBoard;

    [SerializeField]
    private Image blackBoard;

    public Bullet bullet;

    [Header("GameOverUI")]
    [SerializeField]
    private GameObject gameoverBoard;

    [SerializeField]
    private TextMeshProUGUI gameOverScoreText;

    [SerializeField]
    private TextMeshProUGUI gameOverTimeText;

    [SerializeField]
    private TextMeshProUGUI gameOverPlayerLevelText;

    [SerializeField]
    private Button goMenuBtn;

    [Header("UI")]
    public Slider hpbar;
    public Slider fuelbar;

    public GameObject warningArea;

    [Header("Texts")]
    [Space(10f)]
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI highestScoreText;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI levelText;

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

    public static int stage = 1;

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

    public Vector3 defaultCameraPosition;


    private void Awake()
    {

    }

    void Start()
    {
        Invoke(nameof(InitializeGame), 1f);

        StartCoroutine(nameof(IUpdate));
        StartCoroutine(nameof(IAddScore));
        StartCoroutine(nameof(ITimer));
        StartCoroutine(nameof(IFadeOut), 0.5f);

        goMenuBtn.onClick.AddListener(() =>
        {
            SceneManager.LoadScene("Menu");
            Time.timeScale = 1f;
        });
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
        Time.timeScale = 1f;
    }
    public void UpdatePlayerHpBar(float amount)
    {
        hpbar.value = amount;
    }

    public void UpdatePlayerFuelBar(float amount)
    {
        fuelbar.value = amount;
    }

    public void UpdatePlayerLevelText(int level)
    {
        levelText.text = $"{level}Level";
    }

    private IEnumerator IFadeOut(float time)
    {
        float current = 0;
        float percent = 0;

        Color tempColor = blackBoard.color;

        while (percent < 1)
        {
            current += Time.deltaTime;
            percent = current / time;

            tempColor.a = Mathf.Lerp(1, 0, percent);

            blackBoard.color = tempColor;
            yield return null;
        }
        yield return null;

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

    public void FlashEffect(float time)
    {
        StartCoroutine(IFlashEffect(time));

        IEnumerator IFlashEffect(float time)
        {
            float current = 0;
            float percent = 0;
            Color tempColor = whiteBoard.color;

            while (percent < 1)
            {
                current += Time.deltaTime;
                percent = current / (time * 0.3f);//더 빠르게 켜지게 하기 위해 0.3f을 곱함

                tempColor.a = Mathf.Lerp(0, 1, percent);
                whiteBoard.color = tempColor;
                yield return null;
            }

            current = 0; percent = 0;
            while (percent < 1)
            {
                current += Time.deltaTime;
                percent = current / (time * 0.7f);

                tempColor.a = Mathf.Lerp(1, 0, percent);
                whiteBoard.color = tempColor;
                yield return null;
            }
        }
    }

    public void AFlashEffect(float time)
    {
        StartCoroutine(FlashEffect(time));

        IEnumerator FlashEffect(float time)
        {
            float current = 0;
            float percent = 0;
            Color tempColor = whiteBoard.color;

            while (percent < 1)
            {
                current += Time.deltaTime;
                percent = current / (time * 0.3f);

                tempColor.a = Mathf.Lerp(0, 1, percent);
                whiteBoard.color = tempColor;

                yield return null;
            }

            current = 0; percent = 0;
            while (percent < 1)
            {
                current += Time.deltaTime;
                percent = current / (time * 0.7f);

                tempColor.a = Mathf.Lerp(1, 0, percent);
                whiteBoard.color = tempColor;
                yield return null;
            }
        }
    }

    public void SpawnRandomItem(Vector3 pos)
    {
        int randNum = Random.Range(0, 10);

        if (randNum >= 5) randNum = 5;

        Instantiate(itemList[randNum], pos, Quaternion.identity);
    }

    public static bool GetThisChanceResult(float chance)
    {
        if (chance < 0.0000001f)
        {
            chance = 0.0000001f;
        }

        bool success = false;
        int randAccuracy = 1000000;

        float randHitRange = chance * randAccuracy;
        int rand = Random.Range(1, randAccuracy + 1);
        if (rand < randHitRange)
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

        cam.transform.localPosition = defaultCameraPosition;
    }

    private IEnumerator ShakePosition(float range)
    {
        Vector3 shakePosition = defaultCameraPosition + (Vector3)Random.insideUnitCircle * range;

        shakePosition.z = cam.transform.localPosition.z;

        ShakeIntervalMove(shakePosition, 0.1f);
        yield return new WaitForSeconds(0.1f);
    }

    private void ShakeIntervalMove(Vector3 shakePosition, float shakeInterval)
    {
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

    public void GameOver()
    {
        StopAllCoroutines();

        StartCoroutine(IScoreBoardMove(1));

        gameOverPlayerLevelText.text = $"{Player.Instance.Level}Level";
        gameOverScoreText.text = $"Score: {Score}";
        gameOverTimeText.text = $"PlayTime: {Mathf.Round(Timer * 10) * 0.1f}s";
    }

    private IEnumerator IScoreBoardMove(float time)
    {
        float current = 0;
        float percent = 0;
        Vector3 startPos = gameoverBoard.transform.position;
        Vector3 endPos = canvas.transform.position;


        while(percent < 1) 
        {
            current += Time.deltaTime;
            percent = current / time;

            gameoverBoard.transform.position = Vector3.Lerp(startPos, endPos, percent);

            yield return null;
        }
        Time.timeScale = 0.1f;
        yield break;
    }
}
