using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageManager : Singleton<StageManager>
{
    public Sprite[] stageBackGround;
    public SpriteRenderer background;
    public Text stageText;

    public List<string> stageExplain = new List<string>();

    [SerializeField]
    private Image blackBoard;


    private void Start()
    {
        SetStage();
    }

    private void SetStage()
    {
        int stageIndex = GameManager.stage - 1;

        background.sprite = stageBackGround[stageIndex];
        stageText.text = $"{stageExplain[stageIndex]}{stageIndex+1}";

        StartCoroutine(IIFadeOut(1.5f, 2.5f));
    }
    
    /// <summary>
    /// <summary>
    /// 
    /// </summary>
    /// <param name="time">FadeTime</param>
    /// <param name="delay"></param>
    /// <returns></returns>
    private IEnumerator IIFadeOut(float time, float delay = 0)
    {
        float current = 0;
        float percent = 0;
        Color tempColor = blackBoard.color;
        yield return new WaitForSeconds(delay);

        while (percent < 1)
        {
            current += Time.deltaTime;
            percent = current / time;

            tempColor.a = Mathf.Lerp(1, 0, percent);
            blackBoard.color = tempColor;

            yield return null;
        }

        yield break;
    }

}
