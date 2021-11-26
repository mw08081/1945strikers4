using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class F5USubMachine : MonoBehaviour
{
    F5U myPlayer;

    Vector3 dir;
    float lastShotTime;
    [SerializeField] float speed;
    [SerializeField] float dmg;

    private void OnEnable()
    {
        myPlayer = FindObjectOfType<F5U>();
        lastShotTime = 0;
    }

    void Update()
    {
        if(GetComponentInParent<F5UBomb>().isAttackAdmission)
        {
            TraceEnemy();
            if (dir != Vector3.zero)
                BomberAttack();
        }
    }

    void TraceEnemy()
    {
        try
        {
            Transform enemyTransform =  FindObjectOfType<Enemy>().transform;
            dir = enemyTransform.position - transform.position;
        }
        catch(Exception e)
        {
            Debug.Log(e.Message);
            dir = Vector3.zero;
        }
    }
    void BomberAttack()
    {
        if(Time.time - lastShotTime > 0.15f)
        {
            GameObject go = SystemManager.Instance.GetCurrentSceneT<Stage1Scene>().BulletSystem
                .ServeBullet((myPlayer.isP1 ? BulletCode.player1SubBullet2 : BulletCode.player2SubBullet2), transform.position);
            go.GetComponent<Bullet>().Fire((myPlayer.isP1 ? BulletCode.player1SubBullet2 : BulletCode.player2SubBullet2), dir.normalized, speed, dmg);

            lastShotTime = Time.time;
        }
    }
}
