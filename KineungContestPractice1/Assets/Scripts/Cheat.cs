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
        if (Input.GetKeyDown(KeyCode.F2))
        {
            Player.Instance.Level += 1;
        }

        if (Input.GetKeyDown(KeyCode.F3))
        {
            Player.Instance.PetCount += 1;
        }

        if (Input.GetKeyDown(KeyCode.F4))
        {
            if (Player.Instance.IsInvicibility == false)
                Player.Instance.IsInvicibility = true;
            else Player.Instance.IsInvicibility = false;
        }
        if (Input.GetKeyDown(KeyCode.F5))
        {
            
        }
    }
}
