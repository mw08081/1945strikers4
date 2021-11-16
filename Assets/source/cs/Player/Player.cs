using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Actor
{
    [Header("------Player State------")]
    [SerializeField] Animator anim;
    [SerializeField] Vector3 moveDir;
    [SerializeField] float speed;
    [SerializeField] int power;
    [SerializeField] public int bomb;

    [Header("----Attack Info----")]
    [SerializeField] Transform[] firePosition;
    [SerializeField] float bulletSpeed;
    [SerializeField] float dmg;
    [SerializeField] float subBulletSpeed;
    [SerializeField] float subDmg;

    float lastShotTime;
    float lastSubShotTime;
    
    float bombCoolDown;
    bool isBomb;
    bool isThrow;

    public override void Initializing()
    {
        base.hp = 3;

        isBomb = false;
        lastShotTime = Time.time;
    }

    protected override void Updating()
    {
        AssignMoveDirection();
        if(!isBomb)
        {
            UpdateMove();
            Attack();
        }
        ThrowingDownBomb();

        WholeAnimation();
    }

    #region Launch
    
    #endregion 

    #region Moving
    void AssignMoveDirection()
    {
        Vector3 dir = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        moveDir = dir;
    }
    void UpdateMove()
    {
        if (moveDir == Vector3.zero)
            return;

        //LimitPlayerPosition();                                    //This Method is newWay to limit PlayerPosition using Camera's worldToViewport / viewPortToWorld
        moveDir = AdjustMoveDirection(moveDir);

        transform.position += moveDir * speed * Time.deltaTime;
    }

    void LimitPlayerPosition()
    {
        Vector3 pos = Camera.main.WorldToViewportPoint(transform.position);
        
        if (pos.x > 1) { pos.x = 1.0f; }
        if (pos.x < 0) { pos.x = 0.0f; }
        if (pos.y > 1) { pos.y = 1.0f; }
        if (pos.y < 0) { pos.y = 0.0f; }

        transform.position = Camera.main.ViewportToWorldPoint(pos);
    }
    Vector3 AdjustMoveDirection(Vector3 moveDir)
    {
        if (transform.position.x > 6.23f)
            moveDir.x = -1f;
        if (transform.position.x < -6.23f)
            moveDir.x = 1f;
        if (transform.position.z > 18.3f)
            moveDir.z = -1f;
        if (transform.position.z < -6.05f)
            moveDir.z = 1f;

        return moveDir;
    }
    #endregion

    #region Attack
    void Attack()
    {
        if (Input.GetKey(KeyCode.Return) && Time.time - lastShotTime > 0.12f)
        {
            for (int i = 0; i < power; i++)
            {
                int tmp = i;
                if (power == 2)
                    tmp = i + 1;

                GameObject go = SystemManager.Instance.BulletSystem.ServeBullet(BulletCode.player1Bullet, firePosition[tmp].position);

                Bullet bullet = go.GetComponent<Bullet>();
                bullet.Fire(BulletCode.player1Bullet, Vector3.forward, bulletSpeed, dmg);
            }
            lastShotTime = Time.time;
        }
        SubAttack();
    }
    void SubAttack()
    {
        if (Input.GetKey(KeyCode.Return) && Time.time - lastSubShotTime > 0.25)
        {
            try
            {
                GameObject enemy = GameObject.FindObjectOfType<Enemy>().gameObject;
                if (enemy != null)
                {
                    Vector3 dir = enemy.transform.position - transform.position;

                    for (int i = 0; i < 2; i++)
                    {
                        GameObject go = SystemManager.Instance.BulletSystem.ServeBullet(BulletCode.player1SubBullet, firePosition[i + 3].position);

                        Bullet bullet = go.GetComponent<Bullet>();
                        bullet.Fire(BulletCode.player1SubBullet, dir.normalized, bulletSpeed, dmg);
                    }
                }
                lastSubShotTime = Time.time;
            }
            catch (NullReferenceException e)
            {
                Debug.LogWarning(e.Message);
            }
        }
    }
    void ThrowingDownBomb()
    {
        if (Input.GetKeyDown(KeyCode.L) && bomb >= 1 && !isBomb)
        {
            isBomb = true;
            StartCoroutine("ThrowingBomb");
        }
    }
    IEnumerator ThrowingBomb()
    {
        for (int i = 0; i < 360; i += 3)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y + Mathf.Sin(i * Mathf.Deg2Rad) * 0.25f, transform.position.z + Mathf.Cos(i * Mathf.Deg2Rad) * 0.25f);

            if (i > 60 && !isThrow)
            {
                isThrow = true;
                GameObject go = SystemManager.Instance.BulletSystem.ServeBullet(BulletCode.bomb, transform.position);
                Bullet bullet = go.GetComponent<Bullet>();
                bullet.Fire(BulletCode.bomb, Vector3.forward, 4, 2000);
            }
            yield return new WaitForSeconds(0.02f);
        }
        isBomb = false;
        isThrow = false;
        bomb--;
    }
    #endregion

    #region Trigger
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("EnemyBullet"))
        {
            Bullet bullet = other.gameObject.GetComponent<Bullet>();
            OnBulletHitted(bullet.dmg);
        }
        else if (other.gameObject.layer == LayerMask.NameToLayer("Item"))
        {
            GetItem(other.GetComponent<Item>());
        }
    }

    void GetItem(Item item)
    {
        switch (item.itemCode)
        {
            case ItemCode.powerUp:
                if (power < 3)
                    power++;
                SystemManager.Instance.TmpSystem.ServePowerUpTmp(transform.position);
                break;
            case ItemCode.bomb:
                if (bomb < 3)
                    bomb++;
                break;
            case ItemCode.point:
                break;
            default:
                break;
        }
    }

    protected override void OnBulletHitted(float dmg)
    {
        base.OnBulletHitted(dmg);
    }
    protected override void DecreaseHp(float dmg)
    {

        if (power > 1)
            power--;
        else
            base.DecreaseHp(1);

        //hpBar °¨¼Ò
    }
    protected override void OnDead()
    {
        //base.OnDead();

        //gameObject.SetActive(false);
        Debug.Log("DEAD");
    }
    #endregion

    #region Animation
    void WholeAnimation()
    {
        BombAnimation();
        MovingAnimation();
    }

    void BombAnimation()
    {
        if (Input.GetKeyDown(KeyCode.L) && bomb >= 1)
        {
            anim.SetBool("bomb", true);
            bombCoolDown = 1.4f;
        }
        if (anim.GetBool("bomb"))
        {
            bombCoolDown -= Time.deltaTime;
            if (bombCoolDown < 0)
                anim.SetBool("bomb", false);
        }
    }
    void MovingAnimation()
    {
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            anim.SetBool("left", true);
            anim.SetBool("right", false);
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            anim.SetBool("right", true);
            anim.SetBool("left", false);
        }
        else
        {
            anim.SetBool("right", false);
            anim.SetBool("left", false);
        }
    }
    #endregion
}