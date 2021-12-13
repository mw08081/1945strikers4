using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundEnemyTurret : Enemy
{
    [SerializeField] GroundEnemy myBody;
    [SerializeField] bool isMainTurret;

    Player[] targets;
    Transform targetTransform;
    Vector3 setHeadDir;
    float lastPlayerSearchTime;
    const float playerSearchIntervalTime = 7f;

    protected override void Initializing()
    {
        base.Initializing();
        Invoke("FindTarget", 0.3f);
    }

    protected override void Updating()
    {
        if (isDead)
            return;

        base.Updating();
        if (SystemManager.Instance.isForDos && Time.time - lastPlayerSearchTime > playerSearchIntervalTime)
            SetTargetTransform();
        if (targetTransform != null)
        {
            if (isMainTurret)
            {
                if (myBody.isAttackModel1)
                    SethToTarget();
                else if(myBody.isAttackModel2)
                    StartCoroutine("ReadyAttackModel2");
                else
                    return;
            }
            else
                SethToTarget();
        }
            

    }
    void FindTarget()
    {
        targets = FindObjectsOfType<Player>();
        SetTargetTransform();
    }
    void SetTargetTransform()
    {
        targetTransform = targets[Random.Range(0, targets.Length)].transform;
        lastPlayerSearchTime = Time.time;
    }
    
    void SethToTarget()
    {
        setHeadDir = (targetTransform.position - transform.position).normalized;

        if(isMainTurret)
            StartCoroutine("ReadyAttackModel1");
        else
            transform.forward = new Vector3(setHeadDir.x, 0, setHeadDir.z);
    }
    IEnumerator ReadyAttackModel1()
    {
        Vector3 originVec = transform.forward;
        float lookAtAngle;

        for (float progress = 0; progress < 1; progress+=Time.deltaTime)
        {
            lookAtAngle = 360 - (Mathf.Acos(Vector3.Dot(Vector3.right, -transform.forward)) * Mathf.Rad2Deg);
            if (lookAtAngle >= 250 && lookAtAngle <= 290)
                transform.forward = Vector3.Lerp(originVec, new Vector3(setHeadDir.x, 0, setHeadDir.z), progress);
                
            yield return null;
        }

        while (myBody.isAttackModel1)                      //shot
            yield return null;

        originVec = transform.forward;
        for (float progress = 0; progress < 1; progress += Time.deltaTime)
        {
            transform.forward = Vector3.Lerp(originVec, -Vector3.forward, progress);
            yield return null;
        }
    }
    IEnumerator ReadyAttackModel2()
    {
        Vector3 originVec = transform.forward;
        for (float progress = 0; progress < 1; progress += Time.deltaTime)
        {
            transform.forward = Vector3.Lerp(originVec, new Vector3(0, 1, -1), progress);
            yield return null;
        }

        while (myBody.isAttackModel2)                      //shot
            yield return null;

        originVec = transform.forward;
        for (float progress = 0; progress < 1; progress += Time.deltaTime)
        {
            transform.forward = Vector3.Lerp(originVec, -Vector3.forward, progress);
            yield return null;
        }
    }

    #region OnDead
    public override void OnBomb(float val)
    {
        //Block dmged by BF109Bomb
        //base.OnBomb();
    }
    protected override void OnDead()
    {
        base.OnDead();
        SystemManager.Instance.GetCurrentSceneT<InGameScene>().EffectSystem.ServeEffect(EffectCode.tres, transform.position);
        StartCoroutine("FadeOutDestroy");
    }
    IEnumerator FadeOutDestroy()
    {
        Color _color = renderer.material.color;
        while (_color.a > 0)
        {
            _color = renderer.material.color;
            _color.a -= 0.005f;

            renderer.material.color = _color;

            yield return new WaitForSeconds(0.01f);
        }
        Destroy(gameObject);
    }
    #endregion
}
