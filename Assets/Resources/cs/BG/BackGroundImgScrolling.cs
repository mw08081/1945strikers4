using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundImgScrolling : MonoBehaviour
{
    [SerializeField] MeshRenderer bg;
    [SerializeField] float speed;
    [SerializeField] float offsetY;

    void Start()
    {

    }

    void Update()
    {
        BackGroundScrolling();
    }

    void BackGroundScrolling()
    {
        offsetY += (float)speed * Time.deltaTime;
        if (offsetY > 1f)
            offsetY = offsetY % 1.0f;

        Vector2 offset = new Vector2(0, offsetY);

        bg.material.SetTextureOffset("_MainTex", offset);
    }
}
