using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundEnemyLvU2 : GroundEnemy
{
    enum Lv : int
    {
        Lv1 = 0,
        Lv2,
    }

    [Header("----GroundEnemy Field----")]
    [SerializeField] Lv lv;
    bool isFindTarget;

    protected override void Initializing()
    {
        base.Initializing();
        attackInterval = Random.Range(attackIntervalMin, attackIntervalMax);
    }

    protected override void Updating()
    {
        base.Updating();
        if (isDead)
            return;

        if(isAttack)
        {
            if (!isFindTarget)
                FindTarget();
            else
                Attack();
        }
    }

    void FindTarget()
    {
        if(SystemManager.Instance.isForDos)
            playerTransfrom = FindObjectsOfType<Player>()[Random.Range(0, 2)].transform;
        else
            playerTransfrom = FindObjectOfType<Player>().transform;

        if (playerTransfrom != null)
            isFindTarget = true;
    }

    void Attack()
    {
        if (lv == Lv.Lv1)
            attackDir = fireTransform.forward;
        else
        {
            attackDir = (playerTransfrom.position - fireTransform.position).normalized;
            transform.forward = new Vector3(attackDir.x, 0, attackDir.z);
            //StartCoroutine("SetHeadingToAttackDir");
        }

        if(Time.time - lastAttackTime > attackInterval)
        {
            if(Random.Range(0.0f, 1.0f) > (1 - attackProbability))
            {
                StartCoroutine("AttackModelLvU2");
            }

            attackInterval = Random.Range(attackIntervalMin, attackIntervalMax);
            lastAttackTime = Time.time;
        }
    }
    IEnumerator SetHeadingToAttackDir()
    {
        while(true)
        {
            transform.forward = new Vector3(attackDir.x, 0, attackDir.z);

            yield return new WaitForSeconds(0.04f);
        }
    }
    IEnumerator AttackModelLvU2()
    {
        for (int i = 0; i < fireCnt; i++)
        {
            go = SystemManager.Instance.GetCurrentSceneT<InGameScene>().BulletSystem.ServeBullet(BulletCode.enemyBulletM1, fireTransform.position);
            bullet = go.GetComponent<Bullet>();
            bullet.Fire(BulletCode.enemyBulletM1, attackDir, bulletSpeed, dmg);

            yield return new WaitForSeconds(attackIntervalInCoroutine);
        }
    }

    /*
    IEnumerator AttackModelLv3()
    {
        float lookAtAngle = Mathf.Acos(Vector3.Dot(Vector3.right, transform.forward)) * Mathf.Rad2Deg;
        for (int i = -10; i < 20; i+=20)
        {
            attackDir = new Vector3(Mathf.Cos(lookAtAngle + i) * Mathf.Deg2Rad, 0, Mathf.Sin(lookAtAngle + i) * Mathf.Deg2Rad);

            go = SystemManager.Instance.GetCurrentSceneT<InGameScene>().BulletSystem.ServeBullet(BulletCode.enemyBulletM1, fireTransform.position);
            bullet = go.GetComponent<Bullet>();
            bullet.Fire(BulletCode.enemyBulletM1, attackDir, bulletSpeed, dmg);
        }
        yield return new WaitForSeconds(Random.Range(0.3f, 1.0f));
    }
    */
}
