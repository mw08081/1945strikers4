using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EffectPrefabSet
{
    public GameObject effectPrefab;
    public int cnt;
}

public enum EffectCode : int
{
    cero = 0,
    uno,
    dos,
    tres,
    quatro,
    cinco,
}

public class EffectSystem : MonoBehaviour
{
    [SerializeField] EffectPrefabSet[] effectPrefabSets;

    List<Queue<GameObject>> effectQueueList = new List<Queue<GameObject>>();

    void Start()
    {
        GenerateEffect();
    }

    void GenerateEffect()
    {
        for (int i = 0; i < effectPrefabSets.Length; i++)
        {
            effectQueueList.Add(new Queue<GameObject>());
            for(int j = 0; j < effectPrefabSets[i].cnt; j++)
            {
                GameObject go = Instantiate(effectPrefabSets[i].effectPrefab, transform);
                go.SetActive(false);

                effectQueueList[i].Enqueue(go);
            }
        }
    }
        
    void Update()
    {
        
    }

    public GameObject ServeEffect(EffectCode effectCode, Vector3 position)
    {
        if (effectQueueList[(int)effectCode].Count == 0)
        {
            GameObject tmp = Instantiate(effectPrefabSets[(int)effectCode].effectPrefab, transform);
            tmp.SetActive(false);

            effectQueueList[(int)effectCode].Enqueue(tmp);
        }

        GameObject go = effectQueueList[(int)effectCode].Dequeue();
        go.SetActive(true);
        go.transform.position = position;

        return go;
    }

    public void ReturnEffect(EffectCode effectCode, GameObject gameObject)
    {
        gameObject.SetActive(false);
        effectQueueList[(int)effectCode].Enqueue(gameObject);
    }
}
