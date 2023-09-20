using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//EnemyBullet_B is Character Bigger
public class EnemyBullet_B : Bullet
{
    float scale;
    Vector3 localScale;
    Vector3 originScale;

    protected override void Initializing()
    {
        originScale = transform.localScale;
        StartCoroutine("Bigger");
    }

    IEnumerator Bigger()
    {
        scale = 0.01f;
        localScale = transform.localScale;
        float originLocalMag = localScale.magnitude;
        while (localScale.magnitude < originLocalMag * 3)
        {
            transform.localScale = new Vector3(localScale.x + scale, localScale.y + scale, localScale.z + scale);
            scale += 0.1f;

            yield return new WaitForSeconds(0.1f);
        }
    }

    protected override void Resize()
    {
        transform.localScale = originScale;
    }
}
