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
    bool isAttaking;
    public bool isAttackModel1;
    public bool isAttackModel2;
    //Bullet
    GameObject go;
    Bullet bullet;
    #endregion

    protected override void Initializing()
    {
        base.Initializing();
        status = Status.BoxBefore;
        isAttaking = false;
    }
    protected override void Updating()
    {
        base.Updating();
        if (isDead)
            return;

        viewPortPosition = Camera.main.WorldToViewportPoint(transform.position);
        UpdatingMove();
        
        if(status == Status.BoxIn)
            UpdatingAttack();
    }

    #region  Moving
    void UpdatingMove()
    {
        if (viewPortPosition.y > 0.9 && !isFindTarget)
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

        if (targetTransform != null)
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
        if (Time.time - attackModels[0].lastAttackTime > attackModels[0].attackIntervalTime)
        {
            StartCoroutine(attackModels[0].attackModelName);
            attackModels[0].lastAttackTime = Time.time;
        }

        int randAttack = Random.Range(1, 3);
        if(Time.time - attackModels[randAttack].lastAttackTime > attackModels[randAttack].attackIntervalTime && !isAttaking)
        {
            isAttaking = true;
            switch (randAttack)
            {
                case 1:
                    isAttackModel1 = true;
                    break;
                case 2:
                    isAttackModel2 = true;
                    break;
                case 3:
                    break;
                default:
                    break;
            }

            StartCoroutine(attackModels[randAttack].attackModelName);
        }


    }
    IEnumerator attackModel0()
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
    IEnumerator attackModel1()
    {
        int index = 1;
        const float attackModel1DelayTime = 2f;

        yield return new WaitForSeconds(attackModel1DelayTime);
        isAttackModel1 = false;
        for (int i = 0; i < attackModels[index].fireCnt; i++)
        {
            isAttackModel1 = false;
            attackDir = (targetTransform.position - fireTransform[2].position).normalized;
            float lookAtAngle = 360 - (Mathf.Acos(Vector3.Dot(Vector3.right, attackDir)) * Mathf.Rad2Deg);

            for (float a = -10; a < 20; a+=10)
            {
                go = SystemManager.Instance.GetCurrentSceneT<InGameScene>().BulletSystem.ServeBullet(BulletCode.enemyBulletM2, fireTransform[2].position);
                go.GetComponent<Bullet>().Fire(BulletCode.enemyBulletM2, 
                    new Vector3(Mathf.Cos((lookAtAngle + a) * Mathf.Deg2Rad), 0, Mathf.Sin((lookAtAngle + a) * Mathf.Deg2Rad)), attackModels[index].bulletSpeed, 100);
            }
            isAttackModel1 = true;
            yield return new WaitForSeconds(0.1f);
        }
        attackModels[index].lastAttackTime = Time.time;
        isAttackModel1 = false;

        for (int i = 1; i < attackModels.Length; i++)
            attackModels[i].lastAttackTime = Time.time;
        isAttaking = false;
    }
    IEnumerator attackModel2()
    {
        int index = 2;
        const float attackModel2DelayTime = 4f;
        yield return new WaitForSeconds(attackModel2DelayTime);
        isAttackModel2 = false;

        for (int i = 1; i < attackModels.Length; i++)
            attackModels[i].lastAttackTime = Time.time;
        isAttaking = false;
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
