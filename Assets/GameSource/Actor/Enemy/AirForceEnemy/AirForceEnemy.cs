using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirForceEnemy : Enemy
{
    protected override void OnDead()
    {
        base.OnDead();
        if (Random.Range(0.0f, 1.0f) >= (1 - itemDropProbability))
            GameManager.Instance.GetCurrentSceneT<InGameScene>().ItemSystem.ServeItem((ItemCode)Random.Range(0, 2), transform.position);
    }
}
