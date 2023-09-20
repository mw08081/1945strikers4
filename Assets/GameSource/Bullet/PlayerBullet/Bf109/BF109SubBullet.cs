using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BF109SubBullet : Bullet
{
    protected override void Special()
    {
        transform.forward = moveDir;
    }
}
