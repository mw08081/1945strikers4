using UnityEngine;

public class EnemyLv3 : Enemy
{
    [Header("----EnemyLv3 Field----")]
    [SerializeField] Transform[] bulletSpawnPosition;
    [SerializeField] float bulletSpeed;
    Transform playerTransform;

    Vector3 moveArea;
    Vector3 OutPosition;
    bool isOut;
    float lifeTime;

    float attackProbability = 0.65f;
    float attackInterval;
    float lastAttackTime;


    public override void Initializing()
    {
        moveArea = new Vector3(Random.Range(-2.0f, 3.0f), -3.0f, Random.Range(5.0f, 13.5f));
        
        if (Random.Range(0.0f, 1.0f) >= 0.5f) OutPosition = new Vector3(-13.0f, -3, Random.Range(5.0f, 14.5f));
        else OutPosition = new Vector3(13.0f, -3f, Random.Range(5.0f, 14.5f));

        base.Initializing();

        isOut = false;
        lifeTime = 10.0f;

        attackInterval = Random.Range(1.53f, 2.24f);
        lastAttackTime = 0;
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
        if (Time.time - lastAttackTime > attackInterval && Random.Range(0.0f, 1.0f) > (1 - attackProbability))
        {
            for (int i = 0; i < bulletSpawnPosition.Length; i++)
            {
                GameObject go = SystemManager.Instance.BulletSystem.ServeBullet(BulletCode.enemyBulletM2, bulletSpawnPosition[i].position);

                Bullet bullet = go.GetComponent<Bullet>();
                bullet.Fire(BulletCode.enemyBulletM2, (playerTransform.position - bulletSpawnPosition[i].position).normalized , bulletSpeed, dmg); 
            }
            lastAttackTime = Time.time;
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
        SystemManager.Instance.EnemySystem.ReturnEnemy(EnemyCode.lv3, gameObject);
    }

    protected override void OnDead()
    {
        base.OnDead();
        gameObject.SetActive(false);
        SystemManager.Instance.EnemySystem.ReturnEnemy(EnemyCode.lv3, gameObject);
    }
}
