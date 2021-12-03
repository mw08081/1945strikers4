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
        SearchPlayer,
        BoxIn,
        Dead,
        BoxAfter,
    }

    [SerializeField] Lv lv;
    [SerializeField] Status status;
    Vector3 viewPortPosition;


    [SerializeField] Transform fireTransform;
    Transform playerTransfrom;
    [SerializeField] int fireCnt;
    [SerializeField] float bulletSpeed;
    [SerializeField] float attackIntervalInCoroutine;
    [SerializeField] float attackIntervalMax;
    [SerializeField] float attackIntervalMin;
    [SerializeField] float attackProbability;
    GameObject go;
    Bullet bullet;
    Vector3 attackDir;
    float attackInterval;
    float lastAttackTime;

    
    [SerializeField] GameObject destroyObject;
    Transform generateFieldDestroyObject;

    protected override void Initializing()
    {
        base.Initializing();
        status = Status.BoxBefore;

        attackInterval = Random.Range(attackIntervalMin, attackIntervalMax);

        generateFieldDestroyObject = GameObject.Find("EnemySet").transform;
    }

    protected override void Updating()
    {
        //base.Updating();
        if (isDead)
            return;

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
                UpdatingByStatusBoxBefore();
                break;
            case Status.BoxIn:
                UpdatingByStatusBoxIn();
                break;
            case Status.Dead:
                break;
            case Status.BoxAfter:
                Destroy(gameObject);
                break;
            default:
                break;
        }
    }

    void UpdatingByStatusBoxBefore()
    {
        if (SystemManager.Instance.isForDos)
            playerTransfrom = FindObjectsOfType<Player>()[Random.Range(0, 2)].transform;
        else
            playerTransfrom = FindObjectOfType<Player>().transform;

        status = Status.BoxIn;
    }

    void UpdatingByStatusBoxIn()
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
                StartCoroutine("Attack");
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
    IEnumerator Attack()
    {
        for (int i = 0; i < fireCnt; i++)
        {
            go = SystemManager.Instance.GetCurrentSceneT<InGameScene>().BulletSystem.ServeBullet(BulletCode.enemyBulletM1, fireTransform.position);
            bullet = go.GetComponent<Bullet>();
            bullet.Fire(BulletCode.enemyBulletM1, attackDir, bulletSpeed, dmg);

            yield return new WaitForSeconds(attackIntervalInCoroutine);
        }
    }

    void UpdatingByStatusDead()
    {

    }

    protected override void OnDead()
    {
        base.OnDead();
        SystemManager.Instance.GetCurrentSceneT<InGameScene>().EffectSystem.ServeEffect(EffectCode.tres, transform.position);
        GenerateDestroyedObject();

        Destroy(gameObject);
    }

    void GenerateDestroyedObject()
    {
        GameObject destroyedObject = Instantiate(destroyObject, generateFieldDestroyObject);
        destroyedObject.transform.position = transform.position;
        destroyedObject.transform.forward = transform.forward;
    }
}
