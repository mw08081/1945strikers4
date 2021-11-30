using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GameOverScene : BaseScene
{
    [SerializeField] ScoreData scoreData;
    public ScoreData ScoreData
    {
        get { return scoreData; }
    }
 
    protected override void Initializing()
    {
        SystemManager.Instance.CurrentScene = this;

    }

    protected override void Updating()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            ClearGameData();
            SceneController.Instance.ChangeLoadingScene(SceneNameCont.PlayerchoiceScene);
        }
            
    }

    void ClearGameData()
    {
        SystemManager.Instance.ScoreSystem.ClearScore();
        SystemManager.Instance.isForDos = false;

        SystemManager.Instance.Player1PrefabIndex = 2;
        SystemManager.Instance.Player2PrefabIndex = 0;
    }

}
