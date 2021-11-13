using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemManager : MonoBehaviour
{
    static SystemManager instance;
    public static SystemManager Instance
    {
        get
        {
            return instance;
        }
    }
    public float gameStartTime;
    public float gameElapedTime;

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

    ScoreSystem scoreSystem = new ScoreSystem();
    public ScoreSystem ScoreSystem
    {
        get
        {
            return scoreSystem;
        }
    }

    private void Awake()
    {
        if (instance == null)
            instance = this;

    }


    void Start()
    {
        gameStartTime = Time.time;
    }

    void Update()
    {
        gameElapedTime = Time.time - gameStartTime;
    }
}
