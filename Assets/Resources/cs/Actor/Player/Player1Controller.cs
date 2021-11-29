using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player1Controller : MonoBehaviour
{
    [SerializeField] Animator anim;
    Player myPlayer;
    float bombCoolDown;

    void Start()
    {
        anim = GetComponent<Animator>();
        myPlayer = GetComponent<Player>();
    }

    void Update()
    {
        WholeAnimation();
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

    #region Animation
    void WholeAnimation()
    {
        BombAnimation();
        MovingAnimation();
    }

    void BombAnimation()
    {
        if (Input.GetKeyDown(KeyCode.Quote) && myPlayer.bomb >= 1)
        {
            anim.SetBool("bomb", true);
            bombCoolDown = 1.4f;
        }
        if (anim.GetBool("bomb"))
        {
            bombCoolDown -= Time.deltaTime;
            if (bombCoolDown < 0)
                anim.SetBool("bomb", false);
        }
    }
    void MovingAnimation()
    {
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            anim.SetBool("left", true);
            anim.SetBool("right", false);
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            anim.SetBool("right", true);
            anim.SetBool("left", false);
        }
        else
        {
            anim.SetBool("right", false);
            anim.SetBool("left", false);
        }
    }

    #endregion
}
