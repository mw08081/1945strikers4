using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BF109 : Player
{
    [Header("----BF109 Info----")]
    [SerializeField] GameObject subMachinePrefab;
    [SerializeField] Transform[] subMachineSpawnPos;
    [SerializeField] Transform[] firePosition;

    [SerializeField] float bulletSpeed;
    [SerializeField] float dmg;
    [SerializeField] float subBulletSpeed;
    [SerializeField] float subDmg;

    float lastShotTime;
    float lastSubShotTime;
    public float lastSubSpecialShotTime;

    GameObject[] subMachine = new GameObject[2];

    public bool isSubSpecialIn;
    bool isThrow;

    protected override void Initializing()
    {
        base.Initializing();
        lastShotTime = Time.time;
        lastSubShotTime = Time.time;

        isSubSpecialIn = false;
        subMachine[0] = Instantiate(subMachinePrefab);
        subMachine[0].transform.position = transform.position + new Vector3(-2.8f, 0, 0);
        subMachine[0].GetComponent<BF109SubMacine>().SubMachineCodeSet(0);
        subMachine[1] = Instantiate(subMachinePrefab);
        subMachine[1].transform.position = transform.position + new Vector3(2.8f, 0, 0);
        subMachine[0].GetComponent<BF109SubMacine>().SubMachineCodeSet(1);


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
        if(Input.GetKey(KeyCode.Return) && Time.time - lastSubSpecialShotTime > 10.0f)
        {

            isSubSpecialIn = true;
            lastSubSpecialShotTime = 0;

            for (int i = 0; i < subMachine.Length; i++)
                subMachine[i].GetComponent<BF109SubMacine>().SubSpecialAttack();
            lastSubSpecialShotTime = Time.time + 2;
        }
        else if (Input.GetKey(KeyCode.Return) && Time.time - lastSubShotTime > 0.15f && !isSubSpecialIn)
        {
            for (int i = 0; i < subMachine.Length; i++)
                subMachine[i].GetComponent<BF109SubMacine>().SubAttack(subBulletSpeed, subDmg);
            lastSubShotTime = Time.time;


            /*
            try
            {
                GameObject enemy = GameObject.FindObjectOfType<Enemy>().gameObject;
                Vector3 dir;
                if (enemy == null)
                    dir = Vector3.forward;
                else
                    dir = enemy.transform.position;

                for (int i = 0; i < 2; i++)
                {
                    GameObject go = SystemManager.Instance.GetCurrentSceneT<Stage1Scene>().BulletSystem
                        .ServeBullet(BulletCode.player1SubBullet, firePosition[i + 5].position);

                    Bullet bullet = go.GetComponentInChildren<Bullet>();
                    bullet.Fire(BulletCode.player1SubBullet, (dir - firePosition[i + 5].position).normalized, subBulletSpeed, subDmg);
                }
                lastSubShotTime = Time.time;
            }
            catch (NullReferenceException e)
            {
                Debug.LogWarning(e.Message);
            }
            */
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
                GameObject go = SystemManager.Instance.GetCurrentSceneT<Stage1Scene>().BulletSystem.ServeBullet(BulletCode.player1Bomb, transform.position);
                Bullet bullet = go.GetComponent<Bullet>();
                bullet.Fire(BulletCode.player1Bomb, Vector3.forward, 4, 2000);
            }
            yield return new WaitForSeconds(0.02f);
        }
        isBomb = false;
        isThrow = false;
        bomb--;
    }
}
