using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameScene : BaseScene
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
    [SerializeField] Player player2;
    public Player Player2
    {
        get
        {
            return player2;
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

    [SerializeField] AudioSystem audioSystem;
    public AudioSystem AudioSystem
    {
        get
        {
            return audioSystem;
        }
    }

    public bool isBoseDead;

    protected override void Initializing()
    {
        GameManager.Instance.CurrentScene = this;
        gameStartTime = Time.time;
        isBoseDead = false;

        GameObject playerGameObject = Instantiate(PlayerPrefab[GameManager.Instance.Player1PrefabIndex]);
        playerGameObject.AddComponent<Player1Controller>();
        playerGameObject.transform.position = GameManager.Instance.respawnPos;
        player = playerGameObject.GetComponent<Player>();
        player.isP1 = true;
        player.hp = GameManager.Instance.PlayerHp;
        player.power = GameManager.Instance.PlayerPower;

        if (GameManager.Instance.isForDos)
        {
            playerGameObject = Instantiate(PlayerPrefab[GameManager.Instance.Player2PrefabIndex]);
            playerGameObject.AddComponent<Player2Controller>();
            playerGameObject.transform.position = GameManager.Instance.respawnPos;
            player2 = playerGameObject.GetComponent<Player>();
            player2.isP1 = false;
            player2.hp = GameManager.Instance.Player2Hp;
            player2.power = GameManager.Instance.Player2Power;
        }
    }

    protected override void Updating()
    {
        gameElapedTime = Time.time - gameStartTime;

        if (isBoseDead || gameElapedTime > 100 || Input.GetKeyDown(KeyCode.F12))
            NextStage();

        if (GameManager.Instance.isForDos)
        {
            if (player.hp <= 0 && player2.hp <= 0)
                GameOver();
        }
        else
        {
            if (player.hp <= 0)
                GameOver();
        }

        if (Input.GetKeyDown(KeyCode.F) && !GameManager.Instance.isForDos)
        {
            GameManager.Instance.isForDos = true;

            GenerateP2InGame();
        }
    }

    void GenerateP2InGame()
    {
        GameManager.Instance.SetP2PrefabIndex();

        GameObject playerGameObject = Instantiate(PlayerPrefab[GameManager.Instance.Player2PrefabIndex]);
        playerGameObject.AddComponent<Player2Controller>();
        playerGameObject.transform.position = GameManager.Instance.respawnPos;
        player2 = playerGameObject.GetComponent<Player>();
        player2.isP1 = false;
    }

    void NextStage()
    {
        GameManager.Instance.PlayerHp = player.hp;
        GameManager.Instance.PlayerPower = player.power;
        if (GameManager.Instance.isForDos)
        {
            GameManager.Instance.Player2Hp = player2.hp;
            GameManager.Instance.Player2Power = player2.power;
        }

        GameManager.Instance.StageInfo++;
        if(GameManager.Instance.StageInfo == 1)
            SceneController.Instance.ChangeLoadingScene(SceneNameCont.Stage2Scene);
        else
            SceneController.Instance.ChangeLoadingScene(SceneNameCont.Stage3Scene);
    }

    void GameOver()
    {
        SceneController.Instance.ChangeLoadingScene(SceneNameCont.GameOverScene);
    }

    public void ShowOptionPanel()
    {
        PanelSystem.GetPanel(typeof(OptionPanel)).ShowPanel();
        
    }

    public void SetVolume(float value)
    {
        AudioSystem.audioSource.volume = value;
    }
}
