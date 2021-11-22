using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Actor
{
    [SerializeField] float maxHp;
    [SerializeField] protected float speed;
    [SerializeField] protected float dmg;
    [SerializeField] int score;
    [SerializeField] float itemDropProbability;

    protected Renderer renderer;
    Color originColor;
    Color dmgedColor;

    protected override void Initializing()
    {
        renderer = GetComponent<Renderer>();
        if (renderer == null)
            renderer = GetComponentInChildren<Renderer>();

        originColor = renderer.material.GetColor("_Color"); //meshRenderer.material.color;
        dmgedColor = new Color(255, 76, 76, 255);

        hp = maxHp;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("PlayerBullet"))
            OnBulletHitted(other.gameObject.GetComponent<Bullet>().dmg);
        else if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
            OnCrash();
        else if (other.gameObject.layer == LayerMask.NameToLayer("Bomb"))
            OnBulletHitted(500);
    }

    protected override void OnBulletHitted(float dmg)
    {
        base.OnBulletHitted(dmg);
        if (isDead)
            return;
        
        StartCoroutine("HittedEffect");
    }
    IEnumerator HittedEffect()
    {
        bool isDmged = false;
        for(int i = 0; i < 4; i++)
        {
            if (!isDmged)
            {
                renderer.material.color = dmgedColor;
                isDmged = true;
            }
            else
            {
                renderer.material.color = originColor;
                isDmged = false;
            }
            yield return new WaitForSeconds(0.05f);
        }
    }

    protected override void OnCrash()
    {
        DecreaseHp(1250);
    }
    public void OnBomb(float val)
    {
        DecreaseHp(val);
    }

    protected override void DecreaseHp(float dmg)
    {
        base.DecreaseHp(dmg);
    }
    protected override void OnDead()
    {
        base.OnDead();
        renderer.material.color = originColor;
        SystemManager.Instance.ScoreSystem.CalcSc(score);

        if (Random.Range(0.0f, 1.0f) >= (1 - itemDropProbability))
            SystemManager.Instance.GetCurrentSceneT<Stage1Scene>().ItemSystem.ServeItem((ItemCode)Random.Range(0, 2), transform.position);
    }
}
