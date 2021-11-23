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

    public void SaveGameData(float _hp)
    {
        PlayerHp = _hp;
    }
}
