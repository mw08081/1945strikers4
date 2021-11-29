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
    bool isP1;

    void Start()
    {
        if (SystemManager.Instance.GetCurrentSceneT<Stage1Scene>().Player2 == null)
            isP1 = true;
        else
            isP1 = false;
    }

    void Update()
    {
        if (isP1)
        {
            score.text = SystemManager.Instance.ScoreSystem.Player1Score.ToString();
            bombCnt.text = SystemManager.Instance.GetCurrentSceneT<Stage1Scene>().Player.bomb.ToString();

            cnt = (int)SystemManager.Instance.GetCurrentSceneT<Stage1Scene>().Player.hp;
            SetLifeColorAlphaZero();
        }
        else
        {
            score.text = SystemManager.Instance.ScoreSystem.Player2Score.ToString();
            bombCnt.text = SystemManager.Instance.GetCurrentSceneT<Stage1Scene>().Player2.bomb.ToString();

            cnt = (int)SystemManager.Instance.GetCurrentSceneT<Stage1Scene>().Player2.hp;
            SetLifeColorAlphaZero();
        }
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
