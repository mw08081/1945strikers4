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
    [SerializeField] PlayerChoiceImg playerChoiceImg;
    public PlayerChoiceImg PlayerChoiceImg
    {
        get
        {
            return playerChoiceImg;
        }
    }
    public bool isDos;

    protected override void Initializing()
    {
        isDos = false;
        SystemManager.Instance.CurrentScene = this;
    }

    protected override void Updating()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            SystemManager.Instance.PlayerPrefabIndex = player1Choice.index;
            SceneController.Instance.ChangeLoadingScene(SceneNameCont.Stage1Scene);
        }
            
    }

    void InsertPlayer2()
    {
        if (Input.GetKeyDown(KeyCode.F2))
            isDos = true;
    }
}
