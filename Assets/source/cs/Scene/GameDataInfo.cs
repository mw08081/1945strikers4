using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDataInfo : MonoBehaviour
{
    public float Hp { get; set; }
    public float Score { get; set; }

    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AssignGameData(float _hp, float _score)
    {
        Hp = _hp;
        Score = _score;
    }
}
