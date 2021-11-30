using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BulletCode : int
{
    player1Bullet = 0,
    player1SubBullet,
    player1SubBullet2,
    player1Bomb,
    player2Bullet,
    player2SubBullet,
    player2SubBullet2,
    player2Bomb,
    enemyBulletM1,
    enemyBulletM2,
    enemyBulletM3,
}

public class BulletSystem : MonoBehaviour
{
    [Header("----BulletPrefab Data----")]
    //[SerializeField] GameObject[] bulletPrefabList;

    Dictionary<string, GameObject> bulletPrefabCache = new Dictionary<string, GameObject>();

    //Queue<GameObject> playerBulletQueue = new Queue<GameObject>();
    List<Queue<GameObject>> bulletQueueList = new List<Queue<GameObject>>();

    string[,] playerBulletResourcePath = new string[,]
    {
        {"Prefab/Bullet/BF109Bullet/BF109Bullet", "Prefab/Bullet/P38Bullet/P38Bullet", "Prefab/Bullet/F5UBullet/F5UBullet"},
        {"Prefab/Bullet/BF109Bullet/BF109SubBullet", "Prefab/Bullet/P38Bullet/P38SubBullet", "Prefab/Bullet/F5UBullet/F5USubBullet"},
        {"Prefab/Bullet/BF109Bullet/BF109SubBullet2", "Prefab/Bullet/P38Bullet/P38Bomb", "Prefab/Bullet/F5UBullet/F5USubBullet2"},
        {"Prefab/Bullet/BF109Bullet/BF109Bomb", "Prefab/Bullet/P38Bullet/P38Bomber/P38Bomber", "Prefab/Bullet/F5UBullet/F5UBomber"},
    };
    string[,] bulletMakingInfo = new string[,]
    {
        {"30", "null" },                                        //0
        {"30", "null" },                                        //1
        {"15", "null" },                                        //2
        {"2", "null" },                                        //3
        {"30", "Prefab/Bullet/BF109Bullet/BF109Bullet" },      //4
        {"30", "Prefab/Bullet/BF109Bullet/BF109SubBullet" },   //5
        {"15", "Prefab/Bullet/BF109Bullet/BF109SubBullet2" },  //6
        {"2", "Prefab/Bullet/BF109Bullet/BF109Bomb" },         //7
        {"30", "Prefab/Bullet/EnemyBullet/EnemyBulletM1" },    //8
        {"30", "Prefab/Bullet/EnemyBullet/EnemyBulletM2" },    //9
        {"6", "Prefab/Bullet/EnemyBullet/EnemyBulletM3" },    //10
    };

    

    void Start()
    {
        for (int i = 0; i < 4; i++)
            bulletMakingInfo[i, 1] = playerBulletResourcePath[i, SystemManager.Instance.Player1PrefabIndex];
        
        if(SystemManager.Instance.isForDos)                                                             //If isFTP, Instantiate using Player2PrefabIndex
        {
            for (int i = 0; i < 4; i++)
                bulletMakingInfo[i + 4, 1] = playerBulletResourcePath[i, SystemManager.Instance.Player2PrefabIndex];
        }
        else
        {
            if(SystemManager.Instance.Player1PrefabIndex == 0)                                          //Prepare AfterInputFTP
                for (int i = 0; i < 4; i++)                                                                     //when P1 is BF109, set p2BulletModel to F5U
                    bulletMakingInfo[i + 4, 1] = playerBulletResourcePath[i, 2];
        }
        SettingBullet();
    }

    void Update()
    {

    }

    #region ####BulletCache####
    GameObject PrefabLoad(string resourcePath)
    {
        GameObject go = null;

        if (bulletPrefabCache.ContainsKey(resourcePath))
        {
            go = bulletPrefabCache[resourcePath];
        }
        else
        {
            go = Resources.Load<GameObject>(resourcePath);
            if(!go)
            {
                Debug.LogError("Load Error!");
                return null;
            }
            bulletPrefabCache.Add(resourcePath, go);
        }
        GameObject instantiateGo = Instantiate(go, transform);

        return instantiateGo;
    }
    #endregion

    #region ####BulletSetting####
    void SettingBullet()
    {
        for (int i = 0; i < bulletMakingInfo.Length / 2; i++)
        {
            bulletQueueList.Add(new Queue<GameObject>());
            for (int j = 0; j < int.Parse(bulletMakingInfo[i, 0]); j++)
            {
                GameObject go = PrefabLoad(bulletMakingInfo[i,1]);
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
        {
            GameObject go = PrefabLoad(bulletMakingInfo[(int)bulletCode, 1]);
            go.SetActive(false);

            bulletQueueList[(int)bulletCode].Enqueue(go);
        }

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
