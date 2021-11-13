using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmokeGenerator : MonoBehaviour
{
    [SerializeField] GameObject smokePrefab;
    [SerializeField] Transform smokeSpawnTransform;

    float lastSpawnTime = 0;

    void Start()
    {
        
    }
    
    void Update()
    {
        if (Time.time - lastSpawnTime > 3f)
            ServeSmoke();
    }

    void ServeSmoke()
    {
        GameObject smoke = Instantiate<GameObject>(smokePrefab, transform); 
        smoke.transform.position = new Vector3(Random.Range(-6.0f, 6.0f), smokeSpawnTransform.position.y, smokeSpawnTransform.position.z);

        lastSpawnTime = Time.time;
    }
}
