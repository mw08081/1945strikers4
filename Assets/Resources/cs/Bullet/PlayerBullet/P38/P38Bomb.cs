using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P38Bomb : Bullet
{
    Rigidbody bombRb;
    [SerializeField] BulletCode b;

    protected override void Initializing()
    {
        bombRb = GetComponent<Rigidbody>();
    }

    protected override void Special()
    {
        transform.forward = Vector3.Lerp(Vector3.forward, -Vector3.up, (Time.time - generateTime) / 1.5f);
    }
    protected override void Explosive()
    {
        SystemManager.Instance.GetCurrentSceneT<InGameScene>().EffectSystem.ServeEffect(EffectCode.tres, transform.position);

    }

    void InvokeDestroy()
    {

    }
}
