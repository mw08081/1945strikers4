using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BF109Bomb : Bullet
{
    protected override void Initializing()
    {

    }

    protected override void Special()
    {
        transform.forward = Vector3.Lerp(Vector3.forward, GetComponent<Rigidbody>().velocity.normalized, (Time.time - generateTime) / 2.0f);
    }

    protected override void Explosive()
    {
        SystemManager.Instance.GetCurrentSceneT<InGameScene>().EffectSystem.ServeEffect(EffectCode.dos, transform.position);

        Enemy[] enemies = GameObject.FindObjectsOfType<Enemy>();
        for (int i = 0; i < enemies.Length; i++)
        {
            //if(enemies[i] is GroundEnemy)
            //{
            //    GroundEnemy ge = enemies[i] as GroundEnemy;
            //    if(ge.isBoxIn)
            //        enemies[i].OnBomb(dmg);
            //}
            //else
            enemies[i].OnBomb(dmg);
        }
        


        Bullet[] bullets = GameObject.FindObjectsOfType<Bullet>();
        for (int i = 0; i < bullets.Length; i++)
            bullets[i].ReturnGameObject();
    }
}
