using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BF109SubMacine : MonoBehaviour
{
    BF109 myPlayer;

    [SerializeField] Vector3 moveDir;
    [SerializeField] float speed;
    [SerializeField] Transform firePos;

    [SerializeField] Material[] bulletMatList;

    int subMachineCode;
    bool inCorout;


    private void Start()
    {
        myPlayer = FindObjectOfType<BF109>();
    }

    private void Update()
    {
        if(!inCorout)
        {
            if(subMachineCode == 0)
                transform.position = myPlayer.gameObject.transform.position + new Vector3(-2.8f, 0, 0);
            else
                transform.position = myPlayer.gameObject.transform.position + new Vector3(2.8f, 0, 0);
        }
    }

    public void SubMachineCodeSet(int code)
    {
        this.subMachineCode = code;
    }

    public void SubAttack(float _subBulletSpeed, float _subBulletDmg)
    {
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
                    .ServeBullet((myPlayer.isP1 ? BulletCode.player1SubBullet : BulletCode.player2SubBullet), firePos.position);
                Bullet bullet = go.GetComponentInChildren<Bullet>();
                bullet.Fire((myPlayer.isP1 ? BulletCode.player1SubBullet : BulletCode.player2SubBullet), (dir - firePos.position).normalized, _subBulletSpeed, _subBulletDmg);
            }
        }
        catch (NullReferenceException e)
        {
            Debug.LogWarning(e.Message);
        }
    }

    public void SubSpecialAttack()
    {
        inCorout = true;
        StartCoroutine("SubSpecial");
        StartCoroutine("DrawCircle");
    }

    IEnumerator SubSpecial()
    {
        float startTime = Time.time;
        int colorIndex = 0;

        while (Time.time - startTime < 8.0f)
        {
            GameObject go = SystemManager.Instance.GetCurrentSceneT<Stage1Scene>()
                .BulletSystem.ServeBullet((myPlayer.isP1 ? BulletCode.player1SubBullet2 : BulletCode.player2SubBullet2), firePos.position);

            MeshRenderer[] bulletMeshRenderer =  go.GetComponentsInChildren<MeshRenderer>();
            for (int i = 0; i < bulletMeshRenderer.Length; i++)
            {
                bulletMeshRenderer[i].material = bulletMatList[colorIndex];
            }
                
            colorIndex++;
            if (colorIndex > 2)
                colorIndex = 0;

            Bullet bullet = go.GetComponent<Bullet>();
            bullet.Fire((myPlayer.isP1 ? BulletCode.player1SubBullet2 : BulletCode.player2SubBullet2), Vector3.forward, 30, 15);

            yield return new WaitForSeconds(0.08f);
        }

        myPlayer.isSubSpecialIn = false;
        
        inCorout = false;
        myPlayer.lastSubSpecialShotTime = Time.time;
    }

    IEnumerator DrawCircle()
    {
        float xDiff = SystemManager.Instance.GetCurrentSceneT<Stage1Scene>().Player.transform.position.x;
        float zDiff = SystemManager.Instance.GetCurrentSceneT<Stage1Scene>().Player.transform.position.z + 3;
        float distance = 0;
        
        while (inCorout)
        {
            if (subMachineCode == 0)
            {
                for (int i = 0; i < 360; i+=8)
                {
                    transform.position = new Vector3(Mathf.Cos(i * Mathf.Deg2Rad) + xDiff, -3, Mathf.Sin(i * Mathf.Deg2Rad) + zDiff + distance) * 1f;
                    distance+=0.03f;
                    yield return new WaitForSeconds(0.005f);
                }
            }
            else
            {
                for (int i = 180; i < 540; i+=8)
                {
                    transform.position = new Vector3(Mathf.Cos(i * Mathf.Deg2Rad) + xDiff, -3, Mathf.Sin(i * Mathf.Deg2Rad) + +zDiff + distance) * 1f;
                    distance+=0.03f;
                    yield return new WaitForSeconds(0.005f);
                }
            }    
            yield return new WaitForSeconds(0.01f);
        }
    }
}
