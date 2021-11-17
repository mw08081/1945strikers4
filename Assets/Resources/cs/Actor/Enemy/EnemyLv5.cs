using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AttackModelInfo
{
    public string attackModelName;

    public float attackInterval;
    public float lastAttackTime;

    public BulletCode _bulletCode;
    public float _bulletSpeed;
}

public class EnemyLv5 : Enemy
{
    [Header("----EnemyLv5 Field----")]
    [SerializeField] float lifeTime;
    [SerializeField] Transform[] bulletSpawnPosition;
    [SerializeField] AttackModelInfo[] attackModelInfos;
    
    Vector3 moveArea;
    Transform playerTransform;
    bool isArived = false;
    
    protected override void Initializing()
    {
        base.Initializing();
        
        moveArea = new Vector3(Random.Range(-3.0f, 3.0f), -3.0f, Random.Range(11.5f, 15.5f));
        Invoke("UpdateMove", 2f);
    }
    protected override void Updating()
    {
        lifeTime -= Time.deltaTime;
        if (lifeTime < 0 && !isDead)
            ReturnGameObject();

        if(!isArived)
            UpdateInitiating();
        if (!isDead)
        {
            TrackingPlayerTransform();
            UpdateAttack();
        }
    }

    void UpdateInitiating()
    {
        Vector3 refVec = Vector3.zero;
        transform.position = Vector3.SmoothDamp(transform.position, moveArea, ref refVec, 0.1f);
    }
    void UpdateMove() 
    {
        isArived = true;
        StartCoroutine("GotoDestPos");
    }
    IEnumerator GotoDestPos()
    {
        while (true)
        {
            Vector3 destPos = new Vector3(Random.Range(-5.0f, 5.0f), -3f, Random.Range(11.0f, 16.0f));
            while(Vector3.Distance(transform.position, destPos) > 0.1f)
            {
                Vector3 refVec = Vector3.zero;
                transform.position = Vector3.SmoothDamp(transform.position, destPos, ref refVec, 0.1f);

                yield return new WaitForSeconds(0.001f);
            }
            yield return new WaitForSeconds(0.5f);
        }
    }

    void UpdateAttack()
    {
        for (int i = 0; i < attackModelInfos.Length; i++)
        {
            if(Time.time - attackModelInfos[i].lastAttackTime > attackModelInfos[i].attackInterval && !isDead)
            {
                StartCoroutine(attackModelInfos[i].attackModelName);
                attackModelInfos[i].lastAttackTime = Time.time;
            }
        }
    }
    IEnumerator AttackModelA()
    {
        //using AttackModelInfo[0] >> A
        int stAngle = (int)Random.Range(0.0f, 360.0f);
        for (int i = 0; i < 7; i++)
        {
            for (int j = stAngle; j < stAngle + 360; j += 72)
            {
                GameObject go = SystemManager.Instance.GetCurrentSceneT<Stage1Scene>().BulletSystem.ServeBullet(attackModelInfos[0]._bulletCode, bulletSpawnPosition[0].position);

                Bullet bullet = go.GetComponent<Bullet>();
                bullet.Fire(attackModelInfos[0]._bulletCode, new Vector3(Mathf.Cos(j * Mathf.Deg2Rad), 0, Mathf.Sin(j * Mathf.Deg2Rad))  , attackModelInfos[0]._bulletSpeed, 100);

                yield return new WaitForSeconds(0.01f);
            }
            stAngle += 10;
            yield return new WaitForSeconds(0.05f);
        }
    }
    IEnumerator AttackModelB()
    {
        //using AttackModelInfo[1]
        int stAngle = Random.Range(235, 276);
        int finAngle = stAngle + 30;
        for (int i = 0; i < 5; i++)
        {
            for (int j = stAngle; j < finAngle; j += 5)
            {
                GameObject go = SystemManager.Instance.GetCurrentSceneT<Stage1Scene>().BulletSystem.ServeBullet(attackModelInfos[1]._bulletCode, bulletSpawnPosition[0].position);

                Bullet bullet = go.GetComponent<Bullet>();
                bullet.Fire(attackModelInfos[1]._bulletCode, new Vector3(Mathf.Cos(j * Mathf.Deg2Rad), 0, Mathf.Sin(j * Mathf.Deg2Rad)), attackModelInfos[1]._bulletSpeed, 100);
            }
            yield return new WaitForSeconds(0.05f);
        }
    }
    IEnumerator AttackModelC()
    {
        //using AttackModelInfo[2]
        for (int i = 0; i < 2; i++)
        {
            for (int j = 0; j < 2; j++)
            {
                GameObject go = SystemManager.Instance.GetCurrentSceneT<Stage1Scene>().BulletSystem.ServeBullet(attackModelInfos[2]._bulletCode, bulletSpawnPosition[j + 1].position);

                Bullet bullet = go.GetComponent<Bullet>();
                bullet.Fire(attackModelInfos[2]._bulletCode, (playerTransform.position - bulletSpawnPosition[j + 1].position).normalized, attackModelInfos[2]._bulletSpeed, 100);
                yield return new WaitForSeconds(0.01f);
            }
            yield return new WaitForSeconds(1.5f);
        }
    }
    IEnumerator AttackModelD()
    {
        //using AttackModelInfo[3]
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 2; j++)
            {
                GameObject go = SystemManager.Instance.GetCurrentSceneT<Stage1Scene>().BulletSystem.ServeBullet(attackModelInfos[3]._bulletCode, bulletSpawnPosition[j + 3].position);

                Bullet bullet = go.GetComponent<Bullet>();
                bullet.Fire(attackModelInfos[3]._bulletCode, (playerTransform.position - bulletSpawnPosition[j + 3].position).normalized, attackModelInfos[3]._bulletSpeed, 100);
                yield return new WaitForSeconds(0.01f);
            }
            yield return new WaitForSeconds(0.1f);
        }
    }

    void TrackingPlayerTransform()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void ReturnGameObject()
    {
        StartCoroutine("UnAvoidableAttack");
        OnDead();
    }
    IEnumerator UnAvoidableAttack()
    {
        int stAngle = 0;
        for (int j = stAngle; j < 360 + stAngle; j += 5)
        {
            GameObject go = SystemManager.Instance.GetCurrentSceneT<Stage1Scene>().BulletSystem.ServeBullet(BulletCode.enemyBulletM2, bulletSpawnPosition[0].position);

            Bullet bullet = go.GetComponent<Bullet>();
            bullet.Fire(BulletCode.enemyBulletM2, new Vector3(Mathf.Cos(j * Mathf.Deg2Rad), 0, Mathf.Sin(j * Mathf.Deg2Rad)), 40, 100);

            
        }
        yield return new WaitForSeconds(0.001f);
    }

    protected override void OnDead()
    {
        base.OnDead();
        SystemManager.Instance.GetCurrentSceneT<Stage1Scene>().EffectSystem.ServeEffect(EffectCode.tres, transform.position);

        StartCoroutine("FadeOutDestroy");
    }
    IEnumerator FadeOutDestroy()
    {
        Color _color = meshRenderer.material.color;
        while(_color.a > 0)
        {
            _color = meshRenderer.material.color;
            _color.a -= 0.005f;

            meshRenderer.material.color = _color;

            yield return new WaitForSeconds(0.01f);
        }
        SystemManager.Instance.GetCurrentSceneT<Stage1Scene>().isBoseDead = true;
        Destroy(gameObject);
    }
}
