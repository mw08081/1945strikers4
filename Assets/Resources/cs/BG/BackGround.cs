using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGround : MonoBehaviour
{
    [SerializeField]
    float speed;

    void Start()
    {
           
    }

    void Update()
    {
        transform.position -= Vector3.forward * Time.deltaTime * speed;
    }
}
