using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player1Controller : MonoBehaviour
{
    Player myPlayer;

    void Start()
    {
        myPlayer = GetComponent<Player>();
    }

    void Update()
    {
        UpdateMoveDir();
        if (Input.GetKey(KeyCode.Return))
            myPlayer.CallAttackFunc();

        if (Input.GetKey(KeyCode.Quote))
            myPlayer.CallThrowingDownBomb();
    }

    void UpdateMoveDir()
    {
        Vector3 movedir = Vector3.zero;
        if (Input.GetKey(KeyCode.UpArrow))
            movedir.z = 1.0f;
        else if (Input.GetKey(KeyCode.DownArrow))
            movedir.z = -1.0f;
        else
            movedir.z = 0.0f;

        if (Input.GetKey(KeyCode.LeftArrow))
            movedir.x = -1.0f;
        else if (Input.GetKey(KeyCode.RightArrow))
            movedir.x = 1.0f;
        else
            movedir.x = 0.0f;

        movedir = movedir.normalized;

        myPlayer.AssignMoveDirection(movedir);
    }
}
