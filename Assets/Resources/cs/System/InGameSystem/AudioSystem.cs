using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSystem : MonoBehaviour
{
    [SerializeField] GameObject soundModelPrefab;

    Queue<GameObject> fireSoundQueue = new Queue<GameObject>();

    void Start()
    {
        for (int i = 0; i < 30; i++)
        {
            //fireSoundQueue.Enqueue(Instantiate(soundModelPrefab));
        }
    }

    void Update()
    {
        
    }

    public void Gene()
    {
        Instantiate(soundModelPrefab);
    }
}
