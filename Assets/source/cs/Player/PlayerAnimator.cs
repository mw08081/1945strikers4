using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    [SerializeField] Animator anim;
    bool isArrived = false;

    float bombCoolDown;

    void Start()
    {
        
    }

    void Update()
    {
        //Bomb Animation
        if (Input.GetKeyDown(KeyCode.L) && GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().bomb >= 1)
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

        //Moving Animation
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
        
        //Launch Animation
        if(!isArrived)
            transform.position = Vector3.Lerp(transform.position, new Vector3(0, -3, 0), 0.05f);
        if (transform.position.z > -0.1f && !isArrived)
            isArrived = true;
    }
}
