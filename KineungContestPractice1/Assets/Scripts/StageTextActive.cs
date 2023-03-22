using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
public class StageTextActive : MonoBehaviour
{
    public Text stageText;
    public Text explainText;

    public int stage = 1;

    void Start()
    {
        StartCoroutine(IActive(1.8f));
    }

    private IEnumerator IActive(float time)
    {
        float current = 0;
        float percent = 0;

        Color tempColor = stageText.color;
        Color temp1Color = explainText.color;
        
        while (percent < 1)
        {
            current += Time.deltaTime;
            percent = current / time;

            tempColor.a = Mathf.Lerp(0, 1, percent);
            temp1Color.a = Mathf.Lerp(0, 1, percent);

            stageText.color = tempColor;
            explainText.color = temp1Color;

            yield return null;
        }

        yield return new WaitForSeconds(1f);

        current = 0;
        percent = 0;

        while(percent < 1)
        {
            current += Time.deltaTime;
            percent = current / time;

            tempColor.a = Mathf.Lerp(1, 0, percent);
            temp1Color.a = Mathf.Lerp(1, 0, percent);

            stageText.color = tempColor;
            explainText.color = temp1Color;

            yield return null;
        }

        SceneManager.LoadScene("Main");
    }

}
