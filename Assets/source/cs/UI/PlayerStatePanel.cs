using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatePanel : MonoBehaviour
{
    [SerializeField] Text score;
    [SerializeField] Image[] lifes;
    [SerializeField] Text bombCnt;

    int cnt;

    void Start()
    {
        
    }

    void Update()
    {
        score.text = SystemManager.Instance.ScoreSystem.Score.ToString();
        bombCnt.text = SystemManager.Instance.GetCurrentSceneT<Stage1Scene>().Player.bomb.ToString();

        cnt = (int)SystemManager.Instance.GetCurrentSceneT<Stage1Scene>().Player.hp;
        SetLifeColorAlphaZero();
    }

    void SetLifeColorAlphaZero()
    {
        int tmp = 3 - cnt;

        for (int i = 0; i < tmp; i++)
        {
            if(lifes[i].IsActive())
                lifes[i].gameObject.SetActive(false);
        }
    }
}
