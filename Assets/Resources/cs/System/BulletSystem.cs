using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BulletCode : int
{
    player1Bullet = 0,
    player1SubBullet,
    player2Bullet,
    player2SubBullet,
    bomb,
    enemyBulletM1,
    enemyBulletM2,
    enemyBulletM3,
}

public class BulletSystem : MonoBehaviour
{
    [Header("----BulletPrefab Data----")]
    [SerializeField] GameObject[] bulletPrefabList;

    //Queue<GameObject> playerBulletQueue = new Queue<GameObject>();
    List<Queue<GameObject>> bulletQueueList = new List<Queue<GameObject>>();

    void Start()
    {
        SettingBullet();
    }

    void Update()
    {

    }

    #region ####BulletSetting####
    void SettingBullet()
    {
        for (int i = 0; i < bulletPrefabList.Length; i++)
        {
            int cnt;
            switch (i)
            {
                case 0:
                case 1:
                    cnt = 30;
                    break;
                case 2:
                case 3:
                    cnt = 0;
                    break;
                case 4:
                    cnt = 6;
                    break;
                case 5:
                case 6:
                    cnt = 30;
                    break;
                case 7:
                    cnt = 6;
                    break;
                default:
                    cnt = 5;
                    break;
            }

            bulletQueueList.Add(new Queue<GameObject>());
            for (int j = 0; j < cnt; j++)
            {
                GameObject go = Instantiate(bulletPrefabList[i], transform); 
                go.SetActive(false);

                bulletQueueList[i].Enqueue(go);
            }
        }
    }
    #endregion

    #region ####ServeBullet####
    public GameObject ServeBullet(/*Owner owner, */BulletCode bulletCode, Vector3 servePosition)
    {
       
        if (bulletQueueList[(int)bulletCode].Count == 0)
            bulletQueueList[(int)bulletCode].Enqueue(Instantiate(bulletPrefabList[(int)bulletCode], transform));

        GameObject bullet = bulletQueueList[(int)bulletCode].Dequeue();       

        bullet.SetActive(true);
        bullet.transform.position = servePosition;

        return bullet;
    }

    #endregion

    #region ####ReturnBullet####
    public void ReturnBullet(/*Owner owner, */BulletCode bulletCode, GameObject bullet)
    {
        bulletQueueList[(int)bulletCode].Enqueue(bullet);
    }
    #endregion
}
