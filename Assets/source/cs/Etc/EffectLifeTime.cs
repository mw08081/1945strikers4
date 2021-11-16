using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectLifeTime : MonoBehaviour
{
    [SerializeField] EffectCode effectCode;
    [SerializeField] float lifeTime;
    bool isPlay = false;
    float cLifeTime;

    private void Awake()
    {
        cLifeTime = lifeTime;
    }

    private void OnEnable()
    {
        lifeTime = cLifeTime;
        isPlay = true;
    }

    void Update()
    {
        
        if(isPlay)
        {
            lifeTime -= Time.deltaTime;

            if (lifeTime < 0)
                ReturnGameObject();       
        }
    }

    void ReturnGameObject()
    {
        isPlay = false;
        SystemManager.Instance.GetCurrentSceneT<Stage1Scene>().EffectSystem.ReturnEffect(effectCode, gameObject);
    }
}
