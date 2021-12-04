using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundEnemyLvU4 : GroundEnemy
{
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
}
