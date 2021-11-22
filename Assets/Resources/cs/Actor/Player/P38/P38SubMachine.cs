using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P38SubMachine : MonoBehaviour
{
    enum Status : int
    {
        Appear = 0,
        Flip,
        Attack,
        Out,
    }

    [Header("----Appear Info----")]
    [SerializeField] Animator anim;
    [SerializeField] Status status;
    [SerializeField] float speed;
    Vector3 originPos;
    Vector3 attackPos;

    float lastBombDropTime;

    //flip Info
    float flipCoolDown;
    bool isFlip;
    Vector3 refV;

    [Header("----Attack Info----")]
    [SerializeField] Transform[] firePos;
    [SerializeField] float bulletSpeed;
    [SerializeField] float bulletDmg;
    float lastShotTime;
    float elapsedFireTime;

    private void OnEnable()
    {
        status = Status.Appear;
        lastBombDropTime = 0;
        flipCoolDown = 4;
        isFlip = false;
    }

    void Update()
    {
        UpdateMove();
    }

    void UpdateMove()
    {
        switch (status)
        {
            case Status.Appear:
                UpdateMoveStatusAppear();
                break;
            case Status.Flip:
                UpdateMoveStatusFlip();
                break;
            case Status.Attack:
                UpdateMoveStatusAttack();
                break;
            case Status.Out:
                UpdateMoveStatusOut();
                break;
            default:
                break;
        }
    }

    void UpdateMoveStatusAppear()
    {
        transform.position += Vector3.forward * speed * Time.deltaTime;

        if(Time.time - lastBombDropTime > 0.5f)
        {
            GameObject go = SystemManager.Instance.GetCurrentSceneT<Stage1Scene>().BulletSystem.ServeBullet(BulletCode.player1SubBullet2, transform.position);
            go.GetComponent<Bullet>().Fire(BulletCode.player1SubBullet2, Vector3.forward, 0, 0);
            lastBombDropTime = Time.time;
        }

        if (Vector3.Distance(transform.position, originPos) > 30.0f)
            status = Status.Flip;
    }
    void UpdateMoveStatusFlip()
    {
        //anim.SetBool("flip", true);
        //flipCoolDown -= Time.deltaTime;
        //if(flipCoolDown < 0f)
        //    anim.SetBool("flip", false);

        if (!isFlip)
            StartCoroutine("FlipAnimating");

        refV = Vector3.zero;
        transform.position = Vector3.SmoothDamp(transform.position, attackPos, ref refV, 0.1f);

        if (Vector3.Distance(transform.position, attackPos) < 0.1f)
        {
            status = Status.Attack;
            elapsedFireTime = Time.time;
        }
    }
    IEnumerator FlipAnimating()
    {
        isFlip = true;
        for (int angle = 0; angle < 360; angle+=3)
        {
            Vector3 flipPos = new Vector3(0, Mathf.Sin(angle * Mathf.Deg2Rad), Mathf.Cos(angle * Mathf.Deg2Rad));
            transform.position = flipPos * 7 * Time.deltaTime;
            
            yield return new WaitForSeconds(0.008f);
        }
    }

    void UpdateMoveStatusAttack()
    {
        if (Time.time - lastShotTime < 0.07f)
            return;

        try
        {
            Transform enemyTransform = GameObject.FindObjectOfType<Enemy>().transform;
            
            for (int i = 0; i < 2; i++)
            {
                GameObject go = SystemManager.Instance.GetCurrentSceneT<Stage1Scene>().BulletSystem.ServeBullet(BulletCode.player1SubBullet, firePos[i].position);
                go.GetComponent<Bullet>().Fire(BulletCode.player1SubBullet, (enemyTransform.position - transform.position).normalized, bulletSpeed, bulletDmg);
            }
            lastShotTime = Time.time;
        }
        catch(Exception e)
        {
            Debug.Log(e.Message);
        }
        finally
        {
            if (Time.time - elapsedFireTime > 6f)
                status = Status.Out;
        }
    }
    void UpdateMoveStatusOut()
    {
        transform.position += Vector3.forward * speed * Time.deltaTime;

        if (Vector3.Distance(transform.position, attackPos) > 15f)
        {
            SystemManager.Instance.GetCurrentSceneT<Stage1Scene>().BulletSystem.ReturnBullet(BulletCode.player1Bomb, gameObject);
            gameObject.SetActive(false);
        }
            
    }
    public void SettingPos(Vector3 _originPos, Vector3 _attackPos)
    {
        originPos = _originPos;
        attackPos = _attackPos;
    }
}
