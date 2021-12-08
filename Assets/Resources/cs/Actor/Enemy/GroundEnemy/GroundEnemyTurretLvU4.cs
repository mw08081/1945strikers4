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
        if (Time.time - lastAttackTime < 3f)
            return;
        StartCoroutine("AttackCoroutine");
    }

    IEnumerator AttackCoroutine()
    {
        lastAttackTime = Time.time;
        
        Vector3 originLookAtVector = transform.forward;
        Vector3 lookAtVector = new Vector3((targetTransform.position - firePosition.position).x, 0, (targetTransform.position - firePosition.position).z);
        for (float i = 0.1f; i < 0.5f; i+=0.1f)
        {
            transform.forward = Vector3.Lerp(originLookAtVector, lookAtVector, i*2);

            yield return new WaitForSeconds(0.1f);
        }

        yield return new WaitForSeconds(0.01f);
        for (int j = 0; j < myBody.fireCnt; j++)
        {
            for (int i = -5; i < 10; i += 10)
            {

                if (transform.position.z >= targetTransform.position.z)
                    lookAtAngle = 360 - (Mathf.Acos(Vector3.Dot(Vector3.right, transform.forward)) * Mathf.Rad2Deg);
                else
                    lookAtAngle = Mathf.Acos(Vector3.Dot(Vector3.right, transform.forward)) * Mathf.Rad2Deg;
                Debug.Log(lookAtAngle);

                attackDir = new Vector3(Mathf.Cos((lookAtAngle + i) * Mathf.Deg2Rad), 0, Mathf.Sin((lookAtAngle + i) * Mathf.Deg2Rad));

                go = SystemManager.Instance.GetCurrentSceneT<InGameScene>().BulletSystem.ServeBullet(BulletCode.enemyBulletM1, firePosition.position);
                bullet = go.GetComponent<Bullet>();
                bullet.Fire(BulletCode.enemyBulletM1, attackDir, myBody.bulletSpeed, 100);
            }
            yield return new WaitForSeconds(0.4f);
        }

        lastAttackTime = Time.time;
    }
}
