using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLv1 : Enemy
{
    [Header("----EnemyLv1 Field----")]
    [SerializeField] Transform bulletSpawnPosition;
    [SerializeField] float bulletSpeed;

    float lifeTime;

    float attackInterval;
    float attackProbability = 0.9f;
    float lastAttackTime = 0;

    public override void Initializing()
    {
        base.Initializing();
        attackInterval = Random.Range(1.0f, 1.7f);
        lifeTime = 3.0f;
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
            //GameObject go = SystemManager.Instance.BulletSystem.ServeBullet(BulletCode.enemyBulletM1, bulletSpawnPosition.position);
            GameObject go = SystemManager.Instance.GetCurrentSceneT<Stage1Scene>().
                BulletSystem.ServeBullet(BulletCode.enemyBulletM1, bulletSpawnPosition.position);

            Bullet bullet = go.GetComponent<Bullet>();
            bullet.Fire(BulletCode.enemyBulletM1, Vector3.forward * -1, bulletSpeed, dmg);

            lastAttackTime = Time.time;
        }
    }

    void ReturnGameObject()
    {
        gameObject.SetActive(false);
        //SystemManager.Instance.EnemySystem.ReturnEnemy(EnemyCode.lv1, gameObject);
        SystemManager.Instance.GetCurrentSceneT<Stage1Scene>().EnemySystem.ReturnEnemy(EnemyCode.lv1, gameObject);
    }

    protected override void OnDead()
    {
        base.OnDead();
        gameObject.SetActive(false);
        //SystemManager.Instance.EnemySystem.ReturnEnemy(EnemyCode.lv1, gameObject);
        SystemManager.Instance.GetCurrentSceneT<Stage1Scene>().EnemySystem.ReturnEnemy(EnemyCode.lv1, gameObject);

    }
}
