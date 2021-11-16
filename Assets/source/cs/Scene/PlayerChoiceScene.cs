using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerChoiceScene : BaseScene
{

    protected override void Updating()
    {
        if (Input.anyKey)
            SceneController.Instance.ChangeLoadingScene(SceneNameCont.Stage1Scene);
    }
}
