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
        SystemManager.Instance.ScoreSystem.ClearScore();            //���� �ʱ�ȭ
        SystemManager.Instance.isForDos = false;                    //���� 1�ο����� ����

        SystemManager.Instance.Player1PrefabIndex = 2;              //���� ��Ʈ����Ŀ ���� �ʱ�ȭ
        SystemManager.Instance.Player2PrefabIndex = 0;

        SystemManager.Instance.PlayerHp = 3;                        //�÷��̾� ���� �ʱ�ȭ
        SystemManager.Instance.Player2Hp = 3;
        SystemManager.Instance.PlayerPower = 1;
        SystemManager.Instance.Player2Power = 1;

        SystemManager.Instance.StageInfo = 0;                       //�������� ���� �ʱ�ȭ
    }
}