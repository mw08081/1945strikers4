using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundEnemy : Enemy
{
    enum Status : int
    {
        BoxBefore = 0,
        SearchPlayer,
        BoxIn,
        Dead,
        BoxAfter,
    }
    [Header("----GroundEnemy Field----")]
    [SerializeField] Status status;
    Vector3 viewPortPosition;


    [SerializeField] protected Transform fireTransform;
    protected Transform playerTransfrom;
    [SerializeField] public int fireCnt;
    [SerializeField] public float bulletSpeed;
    [SerializeField] protected float attackIntervalInCoroutine;
    [SerializeField] protected float attackIntervalMax;
    [SerializeField] protected float attackIntervalMin;
    [SerializeField] protected float attackProbability;
    protected GameObject go;
    protected Bullet bullet;
    protected Vector3 attackDir;
    protected float attackInterval;
    protected float lastAttackTime;
    public bool isBoxIn;


    [SerializeField] GameObject destroyObject;
    Transform generateFieldDestroyObject;

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
        if (viewPortPosition.y > 1)
            status = Status.BoxBefore;
        else if(viewPortPosition.y > -0.2f)
            status = Status.BoxIn;
        else 
            status = Status.BoxAfter;
            

        switch (status)
        {
            case Status.BoxIn:
                isBoxIn = true;
                break;
            case Status.Dead:
                isBoxIn = false;
                break;
            case Status.BoxAfter:
                Destroy(gameObject);
                break;
            default:
                break;
        }
    }


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
        if (Random.Range(0.0f, 1.0f) >= (1 - itemDropProbability))
            SystemManager.Instance.GetCurrentSceneT<InGameScene>().ItemSystem.ServeItem((ItemCode)Random.Range(0, 2), fireTransform.position);
        
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