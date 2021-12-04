using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundEnemyLvU4 : GroundEnemy
{
    [SerializeField] Renderer turretRenderer;
    [SerializeField] bool isFixed;
    
    bool isFindTarget;

    protected override void Initializing()
    {
        base.Initializing();
        isFixed = false;
        isFindTarget = false;
    }

    protected override void Updating()
    {
        base.Updating();
        if(isBoxIn && !isFixed)
        {
            MoveBody();
        }
    }

    void MoveBody()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
    }

    protected override void OnBulletHitted(float dmg)
    {
        base.OnBulletHitted(dmg);

        StartCoroutine("HittedEffect");
    }
    IEnumerator HittedEffect()
    {
        bool isDmged = false;
        for (int i = 0; i < 4; i++)
        {
            if (!isDmged)
            {
                renderer.material.color = dmgedColor;
                isDmged = true;
            }
            else
            {
                renderer.material.color = originColor;
                isDmged = false;
            }
            yield return new WaitForSeconds(0.05f);
        }
    }
}
