using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GameOverScene : BaseScene
{
    protected override void Initializing()
    {
        GameManager.Instance.CurrentScene = this;

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
