using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cheat : MonoBehaviour
{
    void Update()
    {
        CheatKey();
    }

    private void CheatKey()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            Player.Instance.Hp -= 1;
        }
    }
}
