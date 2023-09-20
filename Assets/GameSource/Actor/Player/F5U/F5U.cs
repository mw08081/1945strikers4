using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class F5U : Player
{
    [Header("----F5U Info----")]
    [SerializeField] Transform[] firePosition;
    [SerializeField] float bulletSpeed;
    [SerializeField] float dmg;
    [SerializeField] float subBulletSpeed;
    [SerializeField] float subDmg;

    float lastShotTime;
    float lastSubShotTime;

    bool isThrow;

    Vector3 i3Aim;
    Vector3 i4Aim;
    Vector3 i5Aim;
    Vector3 i6Aim;

    protected override void Initializing()
    {
        base.Initializing();
        lastShotTime = Time.time;

        i3Aim = new Vector3(-0.07f, 0, 1);
        i4Aim = new Vector3(0.07f, 0, 1);
        i5Aim = new Vector3(-0.1f, 0, 1);
        i6Aim = new Vector3(0.1f, 0, 1);
    }

    protected override void Attack()
    {
        if (Time.time - lastShotTime > 0.09f)
        {
            for (int i = 0; i < power * 2; i++)
            {
                GameObject go = GameManager.Instance.GetCurrentSceneT<InGameScene>().BulletSystem
                    .ServeBullet((isP1 ? BulletCode.player1Bullet : BulletCode.player2Bullet), firePosition[i].position);

                Bullet bullet = go.GetComponent<Bullet>();

                Vector3 aim;
                switch (i)
                {
                    case 0:
                    case 1:
                        aim = Vector3.forward;
                        break;
                    case 2:
                        aim = i3Aim;
                        break;
                    case 3:
                        aim = i4Aim;
                        break;
                    case 4:
                        aim = i5Aim;
                        break;
                    case 5:
                        aim = i6Aim;
                        break;
                    default:
                        aim = Vector3.forward;
                        break;
                }
                bullet.Fire((isP1 ? BulletCode.player1Bullet : BulletCode.player2Bullet), aim, bulletSpeed, dmg);
            }
            lastShotTime = Time.time;
        }
        SubAttack();
    }
    protected override void SubAttack()
    {
        if (Time.time - lastSubShotTime > 0.25)
        {
            try
            {
                GameObject enemy = GameObject.FindObjectOfType<Enemy>().gameObject;
                if (enemy != null)
                {
                    Vector3 dir = enemy.transform.position - transform.position;

                    for (int i = 0; i < 2; i++)
                    {
                        GameObject go = GameManager.Instance.GetCurrentSceneT<InGameScene>().BulletSystem
                            .ServeBullet((isP1 ? BulletCode.player1SubBullet : BulletCode.player2SubBullet), firePosition[i + 3].position);

                        Bullet bullet = go.GetComponent<Bullet>();
                        bullet.Fire((isP1 ? BulletCode.player1SubBullet : BulletCode.player2SubBullet), dir.normalized, subBulletSpeed, subDmg);
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
        if (bomb >= 1 && !isBomb)
        {
            F5UBomb bomber = GameManager.Instance.GetCurrentSceneT<InGameScene>().BulletSystem
                .ServeBullet((isP1 ? BulletCode.player1Bomb : BulletCode.player2Bomb), new Vector3(-4.39f, -3, -10f)).GetComponent<F5UBomb>();
            bomber.SetAppearDistPos((isP1 ? BulletCode.player1Bomb : BulletCode.player2Bomb), -4.39f);

            bomber = GameManager.Instance.GetCurrentSceneT<InGameScene>().BulletSystem
                .ServeBullet((isP1 ? BulletCode.player1Bomb : BulletCode.player2Bomb), new Vector3(3.81f, -3, -10.4f)).GetComponent<F5UBomb>();
            bomber.SetAppearDistPos((isP1 ? BulletCode.player1Bomb : BulletCode.player2Bomb), 3.81f);

            bomb--;
        }
    }
}
