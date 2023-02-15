using Microsoft.Unity.VisualStudio.Editor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Image = UnityEngine.UI.Image;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;

    public static GameManager Instance
    {
        get
        {
            return instance;
        }
    }

    public Image[] hpIcons;

    public Text scoreText;

    public GameObject destroyEffect;

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

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        
    }

    void Update()
    {
        
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

}
