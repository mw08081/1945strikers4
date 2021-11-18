using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class F5USubAttack : Bullet
{
    protected override void Special()
    {
        transform.forward = moveDir;
        bulletSpeed = Mathf.Lerp(0, 35, (Time.time - generateTime) * 2);
    }
}
