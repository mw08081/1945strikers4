using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage1Scene : BaseScene
{
    public float gameStartTime;
    public float gameElapedTime;

    [SerializeField] GameObject[] PlayerPrefab;

    [SerializeField] Player player;
    public Player Player
    {
        get
        {
            return player;
        }
    }

    [SerializeField] BulletSystem bulletSystem;
    public BulletSystem BulletSystem
    {
        get
        {
            return bulletSystem;
        }
    }

    [SerializeField] EnemySystem enemySystem;
    public EnemySystem EnemySystem
    {
        get
        {
            return enemySystem;
        }
    }

    [SerializeField] EffectSystem effectSystem;
    public EffectSystem EffectSystem
    {
        get
        {
            return effectSystem;
        }
    }

    [SerializeField] ItemSystem itemSystem;
    public ItemSystem ItemSystem
    {
        get
        {
            return itemSystem;
        }
    }

    [SerializeField] TmpSystem tmpSystem;
    public TmpSystem TmpSystem
    {
        get
        {
            return tmpSystem;
        }
    }

    public bool isBoseDead;
    
    protected override void Initializing()
    {
        SystemManager.Instance.CurrentScene = this;
        gameStartTime = Time.time;
        isBoseDead = false;

        GameObject playerGameObject = Instantiate(PlayerPrefab[SystemManager.Instance.PlayerPrefabIndex]);
        playerGameObject.transform.position = new Vector3(0, -3, -15);
        player = playerGameObject.GetComponent<Player>();
    }

    protected override void Updating()
    {
        gameElapedTime = Time.time - gameStartTime;

        if (isBoseDead || gameElapedTime > 100)
            NextStage();
        if (player.hp <= 0)
            GameOver();
    }

    void NextStage()
    {
        SystemManager.Instance.SaveGameData(player.hp);
        SceneController.Instance.ChangeLoadingScene(SceneNameCont.Stage2Scene);
    }

    void GameOver()
    {
        SceneController.Instance.ChangeLoadingScene(SceneNameCont.GameOverScene);
    }
}
