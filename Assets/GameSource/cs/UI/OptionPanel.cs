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
        GameManager.Instance.ScoreSystem.ClearScore();            //���� �ʱ�ȭ
        GameManager.Instance.isForDos = false;                    //���� 1�ο����� ����

        GameManager.Instance.Player1PrefabIndex = 2;              //���� ��Ʈ����Ŀ ���� �ʱ�ȭ
        GameManager.Instance.Player2PrefabIndex = 0;

        GameManager.Instance.PlayerHp = 3;                        //�÷��̾� ���� �ʱ�ȭ
        GameManager.Instance.Player2Hp = 3;
        GameManager.Instance.PlayerPower = 1;
        GameManager.Instance.Player2Power = 1;

        GameManager.Instance.StageInfo = 0;                       //�������� ���� �ʱ�ȭ
    }
}
