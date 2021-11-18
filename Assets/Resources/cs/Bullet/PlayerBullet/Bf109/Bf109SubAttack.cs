using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bf109SubAttack : Bullet
{
    protected override void Special()
    {
        transform.forward = moveDir;
    }
}
