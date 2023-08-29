using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseScene : MonoBehaviour
{
    void Start()
    {
        Initializing();
    }

    protected virtual void Initializing() { }

    void Update()
    {
        Updating();
    }

    protected virtual void Updating() { }

}
