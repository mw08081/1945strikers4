using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    protected bool isFired = false;

    //[Header("----Bullet Info----")]
    public BulletCode bulletCode;
    [SerializeField]     protected Vector3 moveDir;
    protected float bulletSpeed;
    protected float generateTime;

    public float dmg;

    private void OnEnable()
    {
        Initializing();
        generateTime = Time.time;
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
        GameManager.Instance.GetCurrentSceneT<InGameScene>().EffectSystem.ServeEffect(EffectCode.cero, transform.position);
        Explosive();

        ReturnGameObject();
    }
    protected virtual void Explosive() { } //For Bullet - Bomb

    public virtual void ReturnGameObject()
    {
        Resize();
        
        GameManager.Instance.GetCurrentSceneT<InGameScene>().BulletSystem.ReturnBullet(bulletCode, gameObject);
        gameObject.SetActive(false);
    }
    protected virtual void Resize() { } //For Bullet - EnemyBullet_B
}
