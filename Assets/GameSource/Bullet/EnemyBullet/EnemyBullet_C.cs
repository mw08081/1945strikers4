using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//EnemyBullet_C is Character ClusterBomb
public class EnemyBullet_C : Bullet
{
    GameObject go;
    [SerializeField] float clusterBulletSpeed;
    [SerializeField] int circleCnt;
    [SerializeField] bool isDoubleCluster;
    const float originLifeTime = 0.8f;
    float lifeTime;
    bool isExplosive;

    protected override void Initializing()
    {
        isExplosive = false;
        lifeTime = originLifeTime;
    }

    protected override void Special()
    {
        lifeTime -= Time.deltaTime;
        if (lifeTime < 0 && !isExplosive)
        {
            isFired = false;
            isExplosive = true;
            Explosive();
        }
            
        if(!isExplosive)
            transform.Rotate(Vector3.right, 3f);
    }

    protected override void Explosive()
    {
        if(!isDoubleCluster)
            StartCoroutine("Clustering");
        else
            StartCoroutine("SecondClustering");
    }
    IEnumerator Clustering()
    {
        int stAngle = 0;
        for (int i = 0; i < circleCnt; i++)
        {
            for (int j = stAngle; j < 360 + stAngle; j+=45)
            {
                go = GameManager.Instance.GetCurrentSceneT<InGameScene>().BulletSystem.ServeBullet(BulletCode.enemyBulletM1, transform.position);
                go.GetComponent<Bullet>().Fire(BulletCode.enemyBulletM1, new Vector3(Mathf.Cos(j * Mathf.Deg2Rad), 0, Mathf.Sin(j * Mathf.Deg2Rad)), clusterBulletSpeed, 100);
            }
            yield return new WaitForSeconds(0.03f);
            stAngle += 5;
        }
        ReturnGameObject();
    }
    IEnumerator SecondClustering()
    {
        yield return null;
        int stAngle = Random.Range(0, 61);
        for (int i = 0; i < circleCnt; i++)
        {
            for (int j = stAngle; j < 360 + stAngle; j += 120)
            {
                go = GameManager.Instance.GetCurrentSceneT<InGameScene>().BulletSystem.ServeBullet(BulletCode.enemyBulletM7, transform.position);
                go.GetComponent<Bullet>().Fire(BulletCode.enemyBulletM7, new Vector3(Mathf.Cos(j * Mathf.Deg2Rad), 0, Mathf.Sin(j * Mathf.Deg2Rad)), clusterBulletSpeed, 100);
            }
        }
        ReturnGameObject();
    }

    protected override void Resize()
    {
        
    }
}

