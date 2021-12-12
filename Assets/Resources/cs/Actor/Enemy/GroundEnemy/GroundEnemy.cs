using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
class AttackModel
{
    public string attackModelName;
    public int fireCnt;
    public float bulletSpeed;
    public float lastAttackTime;
    public float attackIntervalTime;
    public float ellapedTime;
}
public class GroundEnemy : Enemy
{
    #region BasicProperty
    enum Status : int
    {
        BoxBefore = 0,
        PlayerSearch,
        BoxIn,
        Dead,
    }
    [Header("----GroundEnemy Field----")]
    [SerializeField] Status status;
    Vector3 viewPortPosition;
    #endregion

    #region StatusProperty
    //Status.PlayerSearch
    float lastPlayerSearchTime;
    const float playerSearchIntervalTime = 7f;
    //Status.BoxIn
    Vector3 moveVector;
    float lastSteeringTime;
    const float steeringIntervalTime = 5f;
    #endregion

    #region AttackProperty
    //Attack
    [SerializeField] protected Transform[] fireTransform;
    Player[] targets;
    Transform targetTransform;
    Vector3 attackDir;
    [SerializeField] AttackModel[] attackModels;
    bool isFindTarget;
    //Bullet
    GameObject go;
    Bullet bullet;
    #endregion

    protected override void Initializing()
    {
        base.Initializing();
        status = Status.BoxBefore;
    }
    protected override void Updating()
    {
        base.Updating();
        if (isDead)
            return;

        viewPortPosition = Camera.main.WorldToViewportPoint(transform.position);
        UpdatingMove();
        UpdatingAttack();
    }

    #region  Moving
    void UpdatingMove()
    {
        if (viewPortPosition.y > 0.8 && !isFindTarget)
            status = Status.BoxBefore;
        else
        {
            if (!isFindTarget)
                status = Status.PlayerSearch;
            else
                status = Status.BoxIn;
        }


        switch (status)
        {
            case Status.BoxBefore:
                UpdatingByStatusBoxBefore();
                break;
            case Status.PlayerSearch:
                UpdatingByStatusPlayerSearch();
                break;
            case Status.BoxIn:
                UpdatingByStatusBoxIN();
                break;
            case Status.Dead:
                break;
            default:
                break;
        }
    }
    void UpdatingByStatusBoxBefore()
    {
        transform.position += -transform.forward * speed * Time.deltaTime;
    }
    void UpdatingByStatusPlayerSearch()
    {
        targets = FindObjectsOfType<Player>();
        targetTransform = targets[Random.Range(0, targets.Length)].transform;

        if(targetTransform != null)
        {
            status = Status.BoxIn;
            isFindTarget = true;
            lastSteeringTime = Time.time;
        }
    }
    void UpdatingByStatusBoxIN()
    {
        UpdatingByStattusBoxInPlayerSearch();
        UpdatingByStattusBoxInMoving();
    }
    void UpdatingByStattusBoxInPlayerSearch()
    {
        if (!(Time.time - lastPlayerSearchTime > playerSearchIntervalTime) || !SystemManager.Instance.isForDos)
            return;

        targetTransform = targets[Random.Range(0, targets.Length)].transform;
    }
    void UpdatingByStattusBoxInMoving()
    {
        if (viewPortPosition.y > 1)
        {
            moveVector = transform.forward * -1;
            speed = 1;
            lastSteeringTime = Time.time;
        }

        transform.position += moveVector * speed * Time.deltaTime;
        if (Time.time - lastSteeringTime > steeringIntervalTime)
        {
            if (viewPortPosition.x >= 0.5)
            {
                if (Random.Range(0.0f, 1.0f) >= 0.5f)
                    moveVector = transform.forward;
                else
                    moveVector = (transform.forward + transform.right * -1);
            }
            else
            {
                if (Random.Range(0.0f, 1.0f) >= 0.5f)
                    moveVector = transform.forward;
                else
                    moveVector = (transform.forward + transform.right * 1);
            }
            speed = Random.Range(0.0f, 1.0f);
            lastSteeringTime = Time.time;
        }
    }
    #endregion

    #region Attack
    private void UpdatingAttack()
    {
        for (int i = 0; i < attackModels.Length; i++)
        {
            attackModels[i].ellapedTime = Time.time - attackModels[i].lastAttackTime;
            if (Time.time - attackModels[i].lastAttackTime > attackModels[i].attackIntervalTime)
            {
                StartCoroutine(attackModels[i].attackModelName);
                attackModels[i].lastAttackTime = Time.time;

            }
        }
            
                
    }
    IEnumerator attackModel1()
    {
        //attackModelA
        int index = 0;

        for (int i = 0; i < attackModels[index].fireCnt; i++)
        {
            for (int j = 0; j < 2; j++)
            {
                attackDir = (targetTransform.position - fireTransform[j].position).normalized;

                go = SystemManager.Instance.GetCurrentSceneT<InGameScene>().BulletSystem.ServeBullet(BulletCode.enemyBulletM2, fireTransform[j].position);
                go.GetComponent<Bullet>().Fire(BulletCode.enemyBulletM2, attackDir, attackModels[index].bulletSpeed, 100);
            }
            yield return new WaitForSeconds(0.5f);
            attackModels[index].lastAttackTime = Time.time;
        }
    }
    #endregion

    #region Trigger
    protected override void OnBulletHitted(float dmg)
    {
        if (status == Status.BoxBefore)
            return;
        base.OnBulletHitted(dmg);
    }

    protected override void OnDead()
    {
        base.OnDead();
        SystemManager.Instance.GetCurrentSceneT<InGameScene>().EffectSystem.ServeEffect(EffectCode.tres, transform.position);
        StartCoroutine("FadeOutDestroy");
    }
    IEnumerator FadeOutDestroy()
    {
        Color _color = renderer.material.color;
        while (_color.a > 0)
        {
            _color = renderer.material.color;
            _color.a -= 0.005f;

            renderer.material.color = _color;

            yield return new WaitForSeconds(0.01f);
        }
        SystemManager.Instance.GetCurrentSceneT<InGameScene>().isBoseDead = true;
        Destroy(gameObject);
    }
    #endregion
}
