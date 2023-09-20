using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Actor
{
    Renderer renderer;
    [Header("------Player State------")]
    public bool isP1;
    [SerializeField] Vector3 moveDir;
    public float speed;
    [SerializeField] public int power;
    [SerializeField] public int bomb;

    readonly float LAUNCH_DURATION = 1f;

    bool isInvincibility;       //무적상태를 의미

    protected bool isBomb;

    protected override void Initializing()
    {
        renderer = GetComponent<Renderer>();

        isP1 = true;
        hp = 3;
        isBomb = false;

        Launch();
    }

    protected override void Updating()
    {
        if(!isBomb && !isDead)
            UpdateMove();
    }

    #region Launch
    public void Launch()
    {
        if (hp < 1)
            return;

        isDead = true;
        StartCoroutine(LaunchMotion());
        StartCoroutine(LaunchEffect());
    }
    IEnumerator LaunchMotion()
    {
        float launchElpasedTime = 0;
        isInvincibility = true;
        
        while(launchElpasedTime < LAUNCH_DURATION)
        {
            transform.position = Vector3.Lerp(GameManager.Instance.respawnPos, Vector3.zero, launchElpasedTime/ LAUNCH_DURATION);

            yield return null;
        }
        isDead = false;
        yield return new WaitForSeconds(2f);

        isInvincibility = false;
    }
    IEnumerator LaunchEffect()
    {
        yield return new WaitForSeconds(0.1f);

        Color originColor = renderer.material.color;
        Color effectColor = new Color(255, 76, 76, 255);

        while(isInvincibility)
        {
            if (renderer.material.color.g != 76)
                renderer.material.color = effectColor;
            else
                renderer.material.color = originColor;

            yield return new WaitForSeconds(0.07f);
        }
        renderer.material.color = originColor;
    }

    void ReLaunch()
    {
        transform.position = GameManager.Instance.respawnPos;       //부활지점
        Launch();
        
        //isArrived = false;
        isInvincibility = true;
    }
    #endregion 

    #region Moving
    public void AssignMoveDirection(Vector3 _dir)
    {
        moveDir = _dir;
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
    protected virtual void Attack() { }
    protected virtual void SubAttack() { }
    protected virtual void ThrowingDownBomb() { }

    public void CallAttackFunc()
    {
        Attack();
    }
    public void CallThrowingDownBomb()
    {
        ThrowingDownBomb();
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
        else if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            OnCrash();
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
                GameManager.Instance.GetCurrentSceneT<InGameScene>().TmpSystem.ServePowerUpTmp(transform.position);
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
        if (!isInvincibility)
            base.OnBulletHitted(dmg);
        //Debug.Log("OnBulletDead");
    }
    protected override void OnCrash()
    {
        if (!isInvincibility)
            base.DecreaseHp(1);
        //Debug.Log("OnCrashDead");
    }
    protected override void DecreaseHp(float dmg)
    {

        if (power > 1)
            power--;
        else
            base.DecreaseHp(1);
    }
    protected override void OnDead()
    {
        base.OnDead();
        hp--;
        //GameManager.Instance.SaveGameData(hp);
        ReLaunch();

        if (hp <= 0)
            Debug.Log("GAMEOVER!!");
    }
    #endregion

    
}