using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundEnemyTurret : Enemy
{
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
        if(targetTransform != null)
            SethToTarget();
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
        {
            float lookAtAngle = 360 - (Mathf.Acos(Vector3.Dot(Vector3.right, setHeadDir)) * Mathf.Rad2Deg);

            if (lookAtAngle < 250 || lookAtAngle > 290)
                return;
            transform.forward = new Vector3(setHeadDir.x, 0, setHeadDir.z);
        }
        else
            transform.forward = new Vector3(setHeadDir.x, 0, setHeadDir.z);
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
