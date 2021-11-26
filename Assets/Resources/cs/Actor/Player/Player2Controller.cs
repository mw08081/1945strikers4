using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player2Controller : MonoBehaviour
{
    Player myPlayer;

    void Start()
    {
        myPlayer = GetComponent<Player>();
    }

    void Update()
    {
        UpdateMoveDir();
        if(Input.GetKey(KeyCode.F))
            myPlayer.CallAttackFunc();

        if (Input.GetKey(KeyCode.G))
            myPlayer.CallThrowingDownBomb();
    }

    void UpdateMoveDir()
    {
        Vector3 movedir = Vector3.zero;
        if (Input.GetKey(KeyCode.W))
            movedir.z = 1.0f;
        else if (Input.GetKey(KeyCode.S))
            movedir.z = -1.0f;
        else
            movedir.z = 0.0f;

        if (Input.GetKey(KeyCode.A))
            movedir.x = -1.0f;
        else if (Input.GetKey(KeyCode.D))
            movedir.x = 1.0f;
        else
            movedir.x = 0.0f;

        movedir = movedir.normalized;

        myPlayer.AssignMoveDirection(movedir);
    }
}
