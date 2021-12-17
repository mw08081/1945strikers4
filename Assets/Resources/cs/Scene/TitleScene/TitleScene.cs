using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleScene : BaseScene
{
    protected override void Initializing()
    {
        
    }

    protected override void Updating()
    {
        if(Input.GetKeyDown(KeyCode.F1))
            SceneController.Instance.ChangeLoadingScene(SceneNameCont.PlayerchoiceScene);
    }
}

