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

    public int Player1PrefabIndex { get; set; }
    public int Player2PrefabIndex { get; set; }
    public bool isForDos { get; set; }

    ScoreSystem scoreSystem = new ScoreSystem();
    public ScoreSystem ScoreSystem
    {
        get
        {
            return scoreSystem;
        }
    }
    public float PlayerHp { get; set; }
    public float Player2Hp { get; set; }

    public BaseScene CurrentScene { get; set; }

    public T GetCurrentSceneT<T>()
        where T : BaseScene
    {
        return CurrentScene as T;
    }

    private void Awake()
    {
        if (instance == null)
            instance = this;
        DontDestroyOnLoad(this);
    }


    void Start()
    {
        
    }

    void Update()
    {

    }

    public void SaveGameData(float _hp1, float _hp2)
    {
        PlayerHp = _hp1;
        Player2Hp = _hp2;
    }

    public void SetP2PrefabIndex()
    {
        if (Player1PrefabIndex != 0)
            Player2PrefabIndex = 0;
        else
            Player2PrefabIndex = 2;
    }
}
