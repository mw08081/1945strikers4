using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLv4 : Enemy
{
    [Header("----EnemyLv4 Field----")]
    [SerializeField] Transform[] bulletSpawnPosition;
    [SerializeField] float bulletSpeed;
    Transform playerTransform;

    Vector3 moveArea;
    Vector3 OutPosition;
    bool isOut;
    float lifeTime;

    float attackModel1Interval;
    float attackModel2Interval = 2.5f;
    float lastAttackTimeModel1;
    float lastAttackTimeModel2;

    protected override void Initializing()
    {
        moveArea = new Vector3(Random.Range(-2.0f, 3.0f), -3.0f, Random.Range(5.0f, 13.5f));

        if (Random.Range(0.0f, 1.0f) >= 0.5f) OutPosition = new Vector3(-13.0f, -3, Random.Range(5.0f, 14.5f));
        else OutPosition = new Vector3(13.0f, -3f, Random.Range(5.0f, 14.5f));

        base.Initializing();

        isOut = false;
        lifeTime = 10.0f;

        attackModel1Interval = Random.Range(1.53f, 2.24f);
        lastAttackTimeModel1 = 0;
        lastAttackTimeModel2 = 0;
    }
    protected override void Updating()
    {
        lifeTime -= Time.deltaTime;
        if (lifeTime < 0)
            ReturnGameObject();

        TrackingPlayerTransform();
        UpdateMove();
        UpdateAttack();
    }

    void UpdateMove()
    {
        Vector3 refVec = Vector3.zero;
        if (isOut)
            transform.position = Vector3.SmoothDamp(transform.position, OutPosition, ref refVec, 0.1f);
        else
            transform.position = Vector3.SmoothDamp(transform.position, moveArea, ref refVec, 0.1f);
    }

    void UpdateAttack()
    {
        if (Time.time - lastAttackTimeModel1 > attackModel1Interval)
        {
            StartCoroutine("AttackModel1");
            lastAttackTimeModel1 = Time.time;
        }

        if(Time.time - lastAttackTimeModel2 > attackModel2Interval)
        {
            StartCoroutine("AttackModel2");
            lastAttackTimeModel2 = Time.time;
        }
    }

    IEnumerator AttackModel1()
    {
        for(int i = 0; i < 4; i++)
        {
            //GameObject go = SystemManager.Instance.BulletSystem.ServeBullet(BulletCode.enemyBulletM2, bulletSpawnPosition[0].position);
            GameObject go = SystemManager.Instance.GetCurrentSceneT<InGameScene>().
                BulletSystem.ServeBullet(BulletCode.enemyBulletM2, bulletSpawnPosition[0].position);

            Bullet bullet = go.GetComponent<Bullet>();
            bullet.Fire(BulletCode.enemyBulletM2, (playerTransform.position - bulletSpawnPosition[0].position).normalized, bulletSpeed, 100);

            yield return new WaitForSeconds(0.25f);
        }
    }

    IEnumerator AttackModel2()
    {
        for(int i = 0; i < 2; i++)
        {
            //GameObject go = SystemManager.Instance.BulletSystem.ServeBullet(BulletCode.enemyBulletM2, bulletSpawnPosition[i + 1].position);
            GameObject go = SystemManager.Instance.GetCurrentSceneT<InGameScene>()
                .BulletSystem.ServeBullet(BulletCode.enemyBulletM2, bulletSpawnPosition[i + 1].position);

            Bullet bullet = go.GetComponent<Bullet>();
            bullet.Fire(BulletCode.enemyBulletM2, (playerTransform.position - bulletSpawnPosition[i + 1].position).normalized, bulletSpeed, 100);

            yield return new WaitForSeconds(2.5f);
        }
    }

    void TrackingPlayerTransform()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void ReturnGameObject()
    {
        if (!isOut)
        {
            isOut = true;
            lifeTime = 5.0f;
            return;
        }

        gameObject.SetActive(false);
        //SystemManager.Instance.EnemySystem.ReturnEnemy(EnemyCode.lv3, gameObject);
        SystemManager.Instance.GetCurrentSceneT<InGameScene>().EnemySystem.ReturnEnemy(EnemyCode.lv3, gameObject);
    }

    protected override void OnDead()
    {
        base.OnDead();
        gameObject.SetActive(false);
        //SystemManager.Instance.EnemySystem.ReturnEnemy(EnemyCode.lv3, gameObject);
        SystemManager.Instance.GetCurrentSceneT<InGameScene>().EnemySystem.ReturnEnemy(EnemyCode.lv3, gameObject);
    }
}
