using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyCode : int
{
    lv1 = 0,
    lv2,
    lv3,
    lv4,
    lv5
}

public class EnemySystem : MonoBehaviour
{
    [SerializeField] float boseGeneratingTime;
    [SerializeField] float enemyGeneratingTime;
    [SerializeField] GameObject[] enemyPrefabList;
    [SerializeField] bool[] isEnemyGeneration;
    [SerializeField] Transform[] enemySpawnPosition;
    [SerializeField] Transform[] formationSpawnPositions;

    

    List<Queue<GameObject>> enemyQueueList = new List<Queue<GameObject>>();

    float elapsedTime;
    bool isbose = false;
    
    float[,] enemySpawnInfo = new float[5, 4]
    {
        {0, 0, 0, 0 },                      //row 0 : SpawnInterval Info
        {0, 0, 0, 0 },                      //row 1 : LastSpawnTime Info
        {1.0f, 2.0f, 7.1f, 9.55f },         //row 2 : RandomSpawnMinimumInterval
        {2.0f, 3.0f, 8.1f, 13.44f },        //row 3 : RandomSpawnMaximumInterval
        {0.8f, 0.7f, 1.0f, 1.0f }           //row 4 : SpawnProbability Info
    };
    float lastGenerateFormationTime;

    void Start()
    {
        GameObjectSetting();

        for (int i = 0; i < enemySpawnInfo.GetLength(1); i++)
            enemySpawnInfo[0, i] = Random.Range(enemySpawnInfo[2, i], enemySpawnInfo[3, i]);
    }
    void GameObjectSetting()
    {
        for (int i = 0; i < enemyPrefabList.Length; i++)
        {
            int cnt;
            switch (i)
            {
                case 0:
                case 1:
                    cnt = 10;
                    break;
                case 2:
                case 3:
                    cnt = 5;
                    break;
                default:
                    cnt = 0;
                    break;
            }

            enemyQueueList.Add(new Queue<GameObject>());
            for (int j = 0; j < cnt; j++)
            {
                GameObject go = Instantiate(enemyPrefabList[i], transform);
                go.SetActive(false);

                enemyQueueList[i].Enqueue(go);

            }
        }
    }

    void Update()
    {
        elapsedTime = Time.time - SystemManager.Instance.GetCurrentSceneT<InGameScene>().gameStartTime;
     
        if (elapsedTime > boseGeneratingTime)
            GenerateBose();
        else if (elapsedTime > enemyGeneratingTime)
            GenerateEnemy();
    }

    void GenerateEnemy()
    {
        for (int i = 0; i < enemySpawnInfo.GetLength(1); i++)
        {
            if (!isEnemyGeneration[i])
                continue;

            if(Time.time - enemySpawnInfo[1, i] > enemySpawnInfo[0, i]                  //row(1) : lastSpawnTime    //row(2) : spawnInterval
                && Random.Range(0.0f, 1.0f) > (1 - enemySpawnInfo[4, i]))               //row(4) : spawnProbability
            {
                if (i > 1 && elapsedTime < 7)                   //block Generating lv3,4 before elapsedTime(7)
                    break;

                if (i < 2)                                      //lv 1,2 spawn at(0)
                    GenerateEnemyLvUnder2(i);
                else
                    ServeEnemy((EnemyCode)i, enemySpawnPosition[Random.Range(1, 3)].position);              //lv 3,4 spawn at(1) or (2)

                enemySpawnInfo[1, i] = Time.time;
            }
            enemySpawnInfo[0, i] = Random.Range(enemySpawnInfo[2, i], enemySpawnInfo[3, i]);
        }
    }
    void GenerateBose()
    {
        if (!isbose && isEnemyGeneration[4])
        {
            GameObject bose = Instantiate<GameObject>(enemyPrefabList[(int)EnemyCode.lv5]);
            bose.transform.position = enemySpawnPosition[0].position;

            isbose = true;
        }

    }

    void GenerateEnemyLvUnder2(int i)
    {
        if (Time.time - lastGenerateFormationTime > 7f)
        {
            StartCoroutine("GenerateFormationEnemy", i);
            lastGenerateFormationTime = Time.time;
        }
        else
            ServeEnemy((EnemyCode)i, new Vector3(Random.Range(-6.0f, 6.0f), enemySpawnPosition[0].position.y, enemySpawnPosition[0].position.z));
    }
    IEnumerator GenerateFormationEnemy(int i)
    {
        int formationCode = Random.Range(0, 3);
        if (formationCode < 2)
        {
            Transform formationSpawnPos = formationSpawnPositions[Random.Range(0, 2)];
            float randAngle = Random.Range(-1.0f, -0.3f);
            
            for (int j = 0; j < 4; j++)
            {
                GameObject go = ServeEnemy((EnemyCode)i, formationSpawnPos.position);
                go.GetComponent<EnemyLvU2>().UpdateMoveFormation(formationCode, randAngle);                              

                yield return new WaitForSeconds(0.35f);
            }
        }
        else
        {
            Transform formationSpawnPos = formationSpawnPositions[2];

            float randX = Random.Range(-6.0f, 6.0f);
            for (int j = 0; j < 3; j++)
            {
                GameObject go = ServeEnemy((EnemyCode)i, new Vector3((j * 3) + randX, formationSpawnPositions[2].position.y, formationSpawnPositions[2].position.z));
                go.GetComponent<EnemyLvU2>().UpdateMoveFormation(formationCode, 0.0f);
            }
        }
    }

    GameObject ServeEnemy(EnemyCode enemyCode, Vector3 spawnPosition)
    {
        if (enemyQueueList[(int)enemyCode].Count == 0)
            enemyQueueList[(int)enemyCode].Enqueue(Instantiate(enemyPrefabList[(int)enemyCode], transform));

        GameObject go = enemyQueueList[(int)enemyCode].Dequeue();

        go.transform.position = spawnPosition;
        go.SetActive(true);

        return go;
    }

    public void ReturnEnemy(EnemyCode enemyCode, GameObject gameObject)
    {
        enemyQueueList[(int)enemyCode].Enqueue(gameObject);
    }
}
