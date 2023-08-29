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
