using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLv2 : Enemy
{
    [Header("----EnemyLv2 Field----")]
    [SerializeField] Transform[] bulletSpawnPosition;
    [SerializeField] float bulletSpeed;
    
    float lifeTime;

    float attackInterval;
    float attackProbability = 0.75f;
    float lastAttackTime = 0;

    public override void Initializing()
    {
        base.Initializing();
        attackInterval = Random.Range(1.2f, 2.0f);
        lifeTime = 3.5f;
    }
    protected override void Updating()
    {
        lifeTime -= Time.deltaTime;
        if (lifeTime < 0)
            ReturnGameObject();

        UpdateMove();
        UpdateAttack();
    }

    void UpdateMove()
    {
        transform.position += -Vector3.forward * speed * Time.deltaTime;
    }

    void UpdateAttack()
    {
        if(Time.time - lastAttackTime > attackInterval && Random.Range(0.0f, 1.0f) > (1 - attackProbability))
        {
            for(int i = 0; i < bulletSpawnPosition.Length; i++)
            {
                //GameObject go = SystemManager.Instance.BulletSystem.ServeBullet(BulletCode.enemyBulletM1, bulletSpawnPosition[i].position);
                GameObject go = SystemManager.Instance.GetCurrentSceneT<Stage1Scene>().BulletSystem.ServeBullet(BulletCode.enemyBulletM1, bulletSpawnPosition[i].position);

                Bullet bullet = go.GetComponent<Bullet>();
                bullet.Fire(BulletCode.enemyBulletM1, Vector3.forward * -1, bulletSpeed, dmg);
            }
            lastAttackTime = Time.time;
        }
    }

    void ReturnGameObject()
    {
        gameObject.SetActive(false);
        //SystemManager.Instance.EnemySystem.ReturnEnemy(EnemyCode.lv2, gameObject);
        SystemManager.Instance.GetCurrentSceneT<Stage1Scene>().EnemySystem.ReturnEnemy(EnemyCode.lv2, gameObject);
    }

    protected override void OnDead()
    {
        base.OnDead();
        gameObject.SetActive(false);
        //SystemManager.Instance.EnemySystem.ReturnEnemy(EnemyCode.lv2, gameObject);
        SystemManager.Instance.GetCurrentSceneT<Stage1Scene>().EnemySystem.ReturnEnemy(EnemyCode.lv2, gameObject);
    }
}
