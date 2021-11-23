using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLvU2 : Enemy
{
    public enum FormationCode : int
    {
        Cero = 0,
        Uno,
        Dos,
    }

    [Header("----EnemyLvU2 Field----")]
    [SerializeField] EnemyCode enemyCode;
    [SerializeField] FormationCode formationCode;
    [SerializeField] Transform[] bulletSpawnPosition;
    [SerializeField] float bulletSpeed;
    [SerializeField] float attackIntervalMax;
    [SerializeField] float attackIntervalMin;
    [SerializeField] float attackProbability;
    
    float lifeTime;
    float attackInterval;
    float lastAttackTime = 0;

    Vector3 moveDir;
    float randAngle;
    bool isFormation;


    protected override void Initializing()
    {
        base.Initializing();
        attackInterval = Random.Range(attackIntervalMin, attackIntervalMax);
        lifeTime = 3.5f;
        isFormation = false;
    }
    protected override void Updating()
    {
        lifeTime -= Time.deltaTime;
        if (lifeTime < 0)
            ReturnGameObject();

        if (!isFormation)
            UpdateMoveSolo();

        transform.forward = moveDir;
        UpdateAttack();
    }

    void UpdateMoveSolo()
    {
        transform.position += -Vector3.forward * speed * Time.deltaTime;
        moveDir = Vector3.forward;
    }
    public void UpdateMoveFormation(int _formationCode, float _ranAngle)
    {
        randAngle = _ranAngle;
        formationCode = (FormationCode)_formationCode;

        isFormation = true;

        switch (formationCode)
        {
            case FormationCode.Cero:
                StartCoroutine("FormationCodeCero");
                break;
            case FormationCode.Uno:
                StartCoroutine("FormationCodeUno");
                break;
            case FormationCode.Dos:
                StartCoroutine("FormationCodeDos");
                break;
            default:
                break;
        }
    }
    IEnumerator FormationCodeCero()
    {
        Vector3 moveDirRef;
        if (transform.position.x > 0)
        {
            for (int i = 180; i < 540; i++)
            {
                moveDirRef = new Vector3(Mathf.Cos(i * Mathf.Deg2Rad), 0, Mathf.Sin(i * Mathf.Deg2Rad));
                moveDir = -moveDirRef;
                transform.position += moveDirRef * 0.2f;

                yield return new WaitForSeconds(0.015f);
            }
        }
        else
        {
            for (int i = 360; i > 0; i--)
            {
                moveDirRef = new Vector3(Mathf.Cos(i * Mathf.Deg2Rad), 0, Mathf.Sin(i * Mathf.Deg2Rad));
                moveDir = -moveDirRef;
                transform.position += moveDirRef * 0.2f;

                yield return new WaitForSeconds(0.015f);
            }
        }
    }
    IEnumerator FormationCodeUno()
    {
        Vector3 moveVec;
        if (transform.position.x > 0)
            moveVec = (new Vector3(-1f, 0, randAngle).normalized);
        else
            moveVec = (new Vector3(1f, 0, randAngle).normalized);

        while (lifeTime > 0)
        {
            transform.position += moveVec * speed * Time.deltaTime;
            moveDir = -moveVec;

            yield return null;
        }
    }
    IEnumerator FormationCodeDos()
    {
        while (lifeTime > 0)
        {
            transform.position += Vector3.forward * -1 * speed * Time.deltaTime;
            moveDir = Vector3.forward;

            yield return null;
        }
    }
    

    void UpdateAttack()
    {
        if (Time.time - lastAttackTime > attackInterval)
        {
            if(Random.Range(0.0f, 1.0f) >= (1 - attackProbability))
            {
                for (int i = 0; i < bulletSpawnPosition.Length; i++)
                {
                    GameObject go = SystemManager.Instance.GetCurrentSceneT<Stage1Scene>().BulletSystem.ServeBullet(BulletCode.enemyBulletM1, bulletSpawnPosition[i].position);

                    Bullet bullet = go.GetComponent<Bullet>();
                    bullet.Fire(BulletCode.enemyBulletM1, moveDir * -1, bulletSpeed, dmg);
                }
            }
            lastAttackTime = Time.time;
        }
    }

    void ReturnGameObject()
    {
        gameObject.SetActive(false);
        SystemManager.Instance.GetCurrentSceneT<Stage1Scene>().EnemySystem.ReturnEnemy(enemyCode, gameObject);
    }

    protected override void OnDead()
    {
        base.OnDead();
        gameObject.SetActive(false);
        SystemManager.Instance.GetCurrentSceneT<Stage1Scene>().EnemySystem.ReturnEnemy(enemyCode, gameObject);
    }
}
