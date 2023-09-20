using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionPanel : BasePanel
{
    protected override void InitializingPanel()
    {
        base.InitializingPanel();
        ClosePanel();
    }

    public override void ShowPanel()
    {
        base.ShowPanel();
        Time.timeScale = 0f;
    }

    public override void ClosePanel()
    {
        base.ClosePanel();
        Time.timeScale = 1f;
    }

    public void GotoMain()
    {
        ClosePanel();
        PanelSystem.DestroyPanel(typeof(OptionPanel));
        SceneController.Instance.GotoMainScene();
    }

    public void GotoGame()
    {

        ClosePanel();
    }

    void ClearGameData()
    {
        GameManager.Instance.ScoreSystem.ClearScore();            //점수 초기화
        GameManager.Instance.isForDos = false;                    //최초 1인용으로 실행

        GameManager.Instance.Player1PrefabIndex = 2;              //선택 스트라이커 정보 초기화
        GameManager.Instance.Player2PrefabIndex = 0;

        GameManager.Instance.PlayerHp = 3;                        //플레이어 정보 초기화
        GameManager.Instance.Player2Hp = 3;
        GameManager.Instance.PlayerPower = 1;
        GameManager.Instance.Player2Power = 1;

        GameManager.Instance.StageInfo = 0;                       //스테이지 정보 초기화
    }
}
