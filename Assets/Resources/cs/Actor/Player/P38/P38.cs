using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P38 : Player
{
    [Header("----P38 Info----")]
    [SerializeField] Transform[] firePosition;

    [SerializeField] float bulletSpeed;
    [SerializeField] float dmg;
    [SerializeField] float subBulletSpeed;
    [SerializeField] float subDmg;

    float lastShotTime;

    Vector3[, ] bomberPos = new Vector3[,]
    {
        {
            new Vector3(0, -3.0f, -9.81f),
            new Vector3(0, -3.0f, 16.81f),
        },
        {
            new Vector3(-3.25f, -3.0f, -11.31f),
            new Vector3(-6.5f, -3.0f, 12f),
        },
        {
            new Vector3(3.25f, -3.0f, -11.31f),
            new Vector3(6.5f, -3.0f, 12f),
        },
        {
            new Vector3(-6.69f, -3.0f, -12.68f),
            new Vector3(-3.5f, -3.0f, 4f),
        },
        {
            new Vector3(6.69f, -3.0f, -12.68f),
            new Vector3(3.5f, -3.0f, 4),
        },
    };

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
                
            }
            lastShotTime = Time.time;
        }
    }

    //This Striker dont have SubAttack
    /*
    protected override void SubAttack()
    {
        if (Input.GetKey(KeyCode.Return) && Time.time - lastSubShotTime > 0.15f)
        {
            
            lastSubShotTime = Time.time;
        }
    }
    */

    protected override void ThrowingDownBomb()
    {
        if (Input.GetKeyDown(KeyCode.L) && bomb >= 1 && !isBomb)
        {
            for (int i = 0; i < bomberPos.Length / 2; i++)
            {
                GameObject go = SystemManager.Instance.GetCurrentSceneT<Stage1Scene>().BulletSystem.ServeBullet(BulletCode.player1Bomb, bomberPos[i, 0]);
                go.GetComponent<P38SubMachine>().SettingPos(bomberPos[i, 0], bomberPos[i, 1]);
            }
                
            bomb--;
        }
    }
}
