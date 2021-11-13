using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    bool isFired = false;

    //[Header("----Bullet Info----")]
    BulletCode bulletCode;
    protected Vector3 moveDir;
    protected float bulletSpeed;
    protected float generateTime;

    public float dmg;

    private void OnEnable()
    {
        Initializing();
    }
    protected virtual void Initializing() { }

    void Update()
    {
        Special();
        if (isFired)
        {
            transform.position += moveDir * bulletSpeed * Time.deltaTime;

            if (Time.time - generateTime > 2.3f)
                ReturnGameObject();
        }
    }

    protected virtual void Special() { }

    public void Fire(BulletCode _bulletCode, Vector3 _moveDir, float _bulletSpeed, float _dmg)
    {
        bulletCode = _bulletCode;
        bulletSpeed = _bulletSpeed;
        dmg = _dmg;
        moveDir = _moveDir;

        isFired = true;
        generateTime = Time.time;
    }

    private void OnTriggerEnter(Collider other)
    {
        SystemManager.Instance.EffectSystem.ServeEffect(EffectCode.cero, transform.position);

        gameObject.SetActive(false);
        Explosive();

        ReturnGameObject();
    }

    protected virtual void Explosive() { } //For Bullet - Bomb

    public void ReturnGameObject()
    {
        Resize();

        gameObject.SetActive(false);
        SystemManager.Instance.BulletSystem.ReturnBullet(bulletCode, gameObject);
    }

    protected virtual void Resize() { } //For Bullet - EnemyBullet_B
}
