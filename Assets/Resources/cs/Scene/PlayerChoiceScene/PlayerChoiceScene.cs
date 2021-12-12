using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerChoiceScene : BaseScene
{
    [SerializeField] Player1Choice player1Choice;
    public Player1Choice Player1Choice
    {
        get
        {
            return player1Choice;
        }
    }
    [SerializeField] Player2Choice player2Choice;
    public Player2Choice Player2Choice
    {
        get
        {
            return player2Choice;
        }
    }
    [SerializeField] PlayerChoiceImg playerChoiceImg;
    public PlayerChoiceImg PlayerChoiceImg
    {
        get
        {
            return playerChoiceImg;
        }
    }
    public bool isForDos;
    int playerCnt = 0;

    protected override void Initializing()
    {
        isForDos = false;
        SystemManager.Instance.CurrentScene = this;
    }

    protected override void Updating()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            isForDos = true;
            SystemManager.Instance.isForDos = true;
        }
        
        if(isForDos)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                if(playerCnt == 0)
                {
                    SystemManager.Instance.Player1PrefabIndex = player1Choice.index;
                    Player2Choice.gameObject.SetActive(true);
                    playerCnt++;
                }
                else if(playerCnt == 1 && SystemManager.Instance.Player1PrefabIndex == player2Choice.index)
                {
                    return;
                }
                else
                {
                    SystemManager.Instance.Player2PrefabIndex = player2Choice.index;
                    SceneController.Instance.ChangeLoadingScene(SceneNameCont.Stage1Scene);
                }
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                SystemManager.Instance.Player1PrefabIndex = player1Choice.index;
                SceneController.Instance.ChangeLoadingScene(SceneNameCont.Stage1Scene);
            }
        }
        

        
            
    }
}
