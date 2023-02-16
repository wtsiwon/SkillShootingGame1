using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    private static TitleManager instance;
    public static TitleManager Instance
    {
        get => instance;
    }

    public Button gameStartBtn;
    public Button exitBtn;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        OnClick();
    }

    private void Update()
    {
        
    }

    private void OnClick()
    {
        gameStartBtn.onClick.AddListener(() =>
        {
            SceneManager.LoadScene("Main");
        });
    }
}
