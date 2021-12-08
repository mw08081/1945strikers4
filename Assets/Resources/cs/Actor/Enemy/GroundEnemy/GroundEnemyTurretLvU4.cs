using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundEnemyTurretLvU4 : MonoBehaviour
{
    enum Lv : int
    {
        Lv3 = 0,
        Lv4,
    }

    [SerializeField] Lv lv;
    [SerializeField] GroundEnemyLvU4 myBody;
    [SerializeField] Transform firePosition;
    Transform targetTransform;
    bool isFindTarget;

    Vector3 attackDir;
    float lookAtAngle;
    float lastAttackTime;

    GameObject go;
    Bullet bullet;

    private void Start()
    {
        
    }

    private void Update()
    {
        if(myBody.isBoxIn)
        {
            if (!isFindTarget)
                FindTarget();
            else
            {
                Attack();
                
            }
                
        }
    }

    void FindTarget()
    {
        if (SystemManager.Instance.isForDos)
            targetTransform = FindObjectsOfType<Player>()[Random.Range(0, 2)].transform;
        else
            targetTransform = FindObjectOfType<Player>().transform;

        if (targetTransform != null)
            isFindTarget = true;
    }

    void Attack()
    {
        transform.forward = new Vector3((targetTransform.position - firePosition.position).x, 0, (targetTransform.position - firePosition.position).z);
        if (Time.time - lastAttackTime < 5f)
            return;

        //AttackProbability
        
        for (int i = -10; i < 30; i+=20)
        {
            lookAtAngle = Mathf.Acos(Vector3.Dot(Vector3.right, transform.forward));// * Mathf.Rad2Deg;

            attackDir = new Vector3(Mathf.Cos(lookAtAngle + i), 0, Mathf.Sin(lookAtAngle + i));

            go = SystemManager.Instance.GetCurrentSceneT<InGameScene>().BulletSystem.ServeBullet(BulletCode.enemyBulletM1, firePosition.position);
            bullet = go.GetComponent<Bullet>();
            bullet.Fire(BulletCode.enemyBulletM1, attackDir, myBody.bulletSpeed, 100);
        }

        lastAttackTime = Time.time;
        //attackProbability re setting
    }
}
