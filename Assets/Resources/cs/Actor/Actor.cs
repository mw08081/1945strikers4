using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actor : MonoBehaviour
{
    [Header("----Actor state----")]
    public float hp;
    public bool isDead;

    void Start()
    {
        //Initializing();
    }

    void OnEnable()
    {
        Initializing();
        isDead = false;
    }

    // Update is called once per frame
    void Update()
    {
        Updating();
    }

    protected virtual void Initializing()
    {

    }

    protected virtual void Updating()
    {

    }

    protected virtual void OnBulletHitted(float dmg)
    {
        if(!isDead)
            DecreaseHp(dmg);
    }

    protected virtual void OnCrash()
    {

    }   
    
    protected virtual void DecreaseHp(float dmg)
    {
        if (dmg == 1)
            OnDead();
        else if (hp <= dmg && !isDead)
        {
            hp = 0;
            OnDead();
        }
        else
            hp -= dmg;
    }

    protected virtual void OnDead()
    {
        isDead = true;
        SystemManager.Instance.GetCurrentSceneT<InGameScene>().EffectSystem.ServeEffect(EffectCode.uno, transform.position);
    }
}
