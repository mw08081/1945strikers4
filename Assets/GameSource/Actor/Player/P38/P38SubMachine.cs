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

    P38 myPlayer;
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
    int angle;

    [Header("----Attack Info----")]
    [SerializeField] Transform[] firePos;
    [SerializeField] float bulletSpeed;
    [SerializeField] float bulletDmg;
    float lastShotTime;
    float elapsedFireTime;

    private void OnEnable()
    {
        myPlayer = FindObjectOfType<P38>();
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

        if (Time.time - lastBombDropTime > 0.5f)
        {
            GameObject go = GameManager.Instance.GetCurrentSceneT<InGameScene>().BulletSystem
                .ServeBullet((myPlayer.isP1 ? BulletCode.player1SubBullet2 : BulletCode.player2SubBullet2), transform.position);
            go.GetComponent<Bullet>().Fire((myPlayer.isP1 ? BulletCode.player1SubBullet2 : BulletCode.player2SubBullet2), Vector3.forward, 0, 0);
            lastBombDropTime = Time.time;
        }

        if (Vector3.Distance(transform.position, originPos) > 25.0f)
            status = Status.Flip;
    }
    void UpdateMoveStatusFlip()
    {
        anim.SetBool("flip", true);
        flipCoolDown -= Time.deltaTime;
        if (flipCoolDown < 0f)
            anim.SetBool("flip", false);

        if (!isFlip)
            StartCoroutine("FlipAnimating");
        
        if (angle > 300)
        {
            refV = Vector3.zero;
            transform.position = Vector3.SmoothDamp(transform.position, attackPos, ref refV, 0.1f);
        }

        if (Vector3.Distance(transform.position, attackPos) < 0.1f)
        {
            status = Status.Attack;
            elapsedFireTime = Time.time;
        }
    }
    IEnumerator FlipAnimating()
    {
        isFlip = true;
        for (angle = 0; angle < 360; angle++)
        {
            //Vector3 flipPos = new Vector3(0, Mathf.Sin(angle * Mathf.Deg2Rad), Mathf.Cos(angle * Mathf.Deg2Rad));
            //transform.position += flipPos * 5 * Time.deltaTime;

            transform.position = new Vector3(transform.position.x, transform.position.y + Mathf.Sin(angle * Mathf.Deg2Rad) * 0.1f , transform.position.z + Mathf.Cos(angle * Mathf.Deg2Rad) * 0.1f);

            

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
                GameObject go = GameManager.Instance.GetCurrentSceneT<InGameScene>().BulletSystem
                    .ServeBullet((myPlayer.isP1 ? BulletCode.player1SubBullet : BulletCode.player2SubBullet) , firePos[i].position);
                go.GetComponent<Bullet>().Fire((myPlayer.isP1 ? BulletCode.player1SubBullet : BulletCode.player2SubBullet), (enemyTransform.position - transform.position).normalized, bulletSpeed, bulletDmg);
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
            GameManager.Instance.GetCurrentSceneT<InGameScene>().BulletSystem.ReturnBullet((myPlayer.isP1 ? BulletCode.player1Bomb : BulletCode.player2Bomb), gameObject);
            gameObject.SetActive(false);
        }
            
    }
    public void SettingPos(Vector3 _originPos, Vector3 _attackPos)
    {
        originPos = _originPos;
        attackPos = _attackPos;
    }
}
