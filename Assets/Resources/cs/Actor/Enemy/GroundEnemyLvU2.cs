using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundEnemyLvU2 : Enemy
{
    enum Lv : int
    {
        Lv1 = 0,
        Lv2 = 1,
    }

    enum Status : int
    {
        BoxBefore = 0,
        BoxIn,
        Dead,
        BoxAfter,
    }

    [SerializeField] Lv lv;
    [SerializeField] Status status;
    [SerializeField] Transform fireTransform;
    [SerializeField] float bulletSpeed;
    [SerializeField] float attackIntervalMax;
    [SerializeField] float attackIntervalMin;
    [SerializeField] float attackProbability;

    Transform playerTransfrom;
    Vector3 viewPortPosition;
    Vector3 attackDir;

    float attackInterval;
    float lastAttackTime;

    protected override void Initializing()
    {
        base.Initializing();
        status = Status.BoxBefore;

        //if (SystemManager.Instance.isForDos)
        //    playerTransfrom = FindObjectsOfType<Player>()[Random.Range(0, 2)].transform;
        //else
        //    playerTransfrom = FindObjectOfType<Player>().transform;

        attackInterval = Random.Range(attackIntervalMin, attackIntervalMax);
    }

    protected override void Updating()
    {
        //base.Updating();

        viewPortPosition = Camera.main.WorldToViewportPoint(transform.position);
        if (viewPortPosition.y < 1)
            status = Status.BoxIn;
        else if (viewPortPosition.y < 0)
            status = Status.BoxAfter;
        else
            status = Status.BoxBefore;

        switch (status)
        {
            case Status.BoxBefore:
                playerTransfrom = UpdatingByStatusBoxBefore();
                break;
            case Status.BoxIn:
                UpdatingByStatusBoxIn();
                break;
            case Status.Dead:
                break;
            case Status.BoxAfter:
                break;
            default:
                break;
        }
    }

    Transform UpdatingByStatusBoxBefore()
    {
        if (SystemManager.Instance.isForDos)
            return FindObjectsOfType<Player>()[Random.Range(0, 2)].transform;
        else
            return FindObjectOfType<Player>().transform;
    }

    void UpdatingByStatusBoxIn()
    {
        if (lv == Lv.Lv1)
            attackDir = ((new Vector3(0, playerTransfrom.position.y, 0)) + transform.forward).normalized;
        //attackDir = ((new Vector3(0, playerTransfrom.position.y, 0)) + transform.forward).normalized;
        else
        {
            attackDir = (playerTransfrom.position - transform.position).normalized;
            transform.forward = attackDir;
        }

        if(Time.time - lastAttackTime > attackInterval)
        {
            if(Random.Range(0.0f, 1.0f) > (1 - attackProbability))
            {
                GameObject go = SystemManager.Instance.GetCurrentSceneT<InGameScene>().BulletSystem.ServeBullet(BulletCode.enemyBulletM1, fireTransform.position);
                
                Bullet bullet = go.GetComponent<Bullet>();
                bullet.Fire(BulletCode.enemyBulletM1, attackDir, bulletSpeed, dmg);
            }

            attackInterval = Random.Range(attackIntervalMin, attackIntervalMax);
            lastAttackTime = Time.time;
        }
    }
}
