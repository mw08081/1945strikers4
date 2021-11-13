using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//EnemyBullet_R is Character Rotating
public class EnemyBullet_R : Bullet
{
    protected override void Special()
    {
        transform.Rotate(Vector3.right, 3f);
    }
}
