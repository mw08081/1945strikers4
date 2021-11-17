using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BF109 : Player
{
    [Header("----BF109 Info----")]
    [SerializeField] Transform[] firePosition;
    [SerializeField] float bulletSpeed;
    [SerializeField] float dmg;
    [SerializeField] float subBulletSpeed;
    [SerializeField] float subDmg;

    float lastShotTime;
    float lastSubShotTime;

    bool isThrow;

    protected override void Initializing()
    {
        base.Initializing();
        lastShotTime = Time.time;
    }

    protected override void Attack()
    {
        if (Input.GetKey(KeyCode.Return) && Time.time - lastShotTime > 0.12f)
        {
            for (int i = 0; i < power; i++)
            {
                int tmp = i;
                if (power == 2)
                    tmp = i + 1;

                GameObject go = SystemManager.Instance.GetCurrentSceneT<Stage1Scene>().BulletSystem.ServeBullet(BulletCode.player1Bullet, firePosition[tmp].position);

                Bullet bullet = go.GetComponent<Bullet>();
                bullet.Fire(BulletCode.player1Bullet, Vector3.forward, bulletSpeed, dmg);
            }
            lastShotTime = Time.time;
        }
        SubAttack();
    }
    protected override void SubAttack()
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
                        GameObject go = SystemManager.Instance.GetCurrentSceneT<Stage1Scene>().BulletSystem.ServeBullet(BulletCode.player1SubBullet, firePosition[i + 3].position);

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
    protected override void ThrowingDownBomb()
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
                GameObject go = SystemManager.Instance.GetCurrentSceneT<Stage1Scene>().BulletSystem.ServeBullet(BulletCode.bomb, transform.position);
                Bullet bullet = go.GetComponent<Bullet>();
                bullet.Fire(BulletCode.bomb, Vector3.forward, 4, 2000);
            }
            yield return new WaitForSeconds(0.02f);
        }
        isBomb = false;
        isThrow = false;
        bomb--;
    }
}
