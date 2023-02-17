using Microsoft.Unity.VisualStudio.Editor;
using System.Collections;
using System.Collections.Generic;
using Unity.Profiling;
using UnityEngine;
using UnityEngine.UI;
using Image = UnityEngine.UI.Image;

public class GameManager : Singleton<GameManager>
{
    public Image[] hpIcons;

    public Text scoreText;

    public GameObject destroyEffect;

    public List<Item> itemList = new List<Item>();

    private Player player;

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
        player = Player.Instance;
        Score = 0;
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

    private void RandomItem(int itemNum)
    {

    }
}
