using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    bool isArrived = false;

    

    void Start()
    {
        
    }

    void Update()
    {
        //Bomb Animation
        

        //Moving Animation
        
        
        //Launch Animation
        if(!isArrived)
            transform.position = Vector3.Lerp(transform.position, new Vector3(0, -3, 0), 0.05f);
        if (transform.position.z > -0.1f && !isArrived)
            isArrived = true;
    }
}
