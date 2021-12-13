using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet_V : Bullet
{

    protected override void Special()
    {
        transform.forward = moveDir;
    }



}