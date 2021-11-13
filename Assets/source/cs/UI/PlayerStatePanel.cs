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
        bombCnt.text = SystemManager.Instance.Player.bomb.ToString();

        cnt = (int)SystemManager.Instance.Player.hp;
    }

    void SetColorAlphaZero()
    {
        int tmp = 3 - cnt;

        for (int i = 0; i < tmp; i++)
        {
            lifes[i].gameObject.SetActive(false);
        }
    }
}
