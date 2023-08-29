using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GameOverScene : BaseScene
{
    protected override void Initializing()
    {
        SystemManager.Instance.CurrentScene = this;

    }

    protected override void Updating()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            ClearGameData();
            PanelSystem.DestroyPanel(typeof(OptionPanel));

            SceneController.Instance.GotoMainScene();
        }
            
    }

    void ClearGameData()
    {
        SystemManager.Instance.ScoreSystem.ClearScore();            //점수 초기화
        SystemManager.Instance.isForDos = false;                    //최초 1인용으로 실행

        SystemManager.Instance.Player1PrefabIndex = 2;              //선택 스트라이커 정보 초기화
        SystemManager.Instance.Player2PrefabIndex = 0;

        SystemManager.Instance.PlayerHp = 3;                        //플레이어 정보 초기화
        SystemManager.Instance.Player2Hp = 3;
        SystemManager.Instance.PlayerPower = 1;
        SystemManager.Instance.Player2Power = 1;

        SystemManager.Instance.StageInfo = 0;                       //스테이지 정보 초기화
    }

}
