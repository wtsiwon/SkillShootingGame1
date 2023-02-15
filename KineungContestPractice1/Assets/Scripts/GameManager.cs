using Microsoft.Unity.VisualStudio.Editor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;

    public static GameManager Instance;

    public Image[] hpIcons;


    private int hp;
    public int Hp
    {
        get
        {
            return hp;
        }
        set
        {
            if(value <= 0)
            {

            }
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
}
