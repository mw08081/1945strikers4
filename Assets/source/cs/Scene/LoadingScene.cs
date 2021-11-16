using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class LoadingScene : BaseScene
{
    [SerializeField] Text loadingWaitStrTxt;
    const string loadingWaitStr = "Loading...";

    const float charPrintInterval = 0.15f;
    float lastPrintTime;

    float startTime;
    int idx;

    bool isCalled = false;

    protected override void Initializing()
    {
        startTime = Time.time;
        idx = 1;

        //Originaly i should call CallNextScene() here,
        //But i dont need to time to gameLoad becuase this GameData isnt big
        //So For just show LoadingScene, i call CallNextScene() in Update() after 3seconds

        /*
        if(!isCalled)
        {
            isCalled = true;
            SceneController.Instance.CallNextScene();
        }
        */
    }

    protected override void Updating()
    {
        UpdatingStr();

        if (Time.time - startTime > 1f && !isCalled)
        {
            isCalled = true;
            SceneController.Instance.CallNextScene();
        }
            
    }

    void UpdatingStr()
    {
        if (Time.time - lastPrintTime > charPrintInterval)
        {
            loadingWaitStrTxt.text = loadingWaitStr.Substring(0, idx++);

            if (idx > loadingWaitStr.Length)
                idx = 0;

            lastPrintTime = Time.time;
        }
    }
}
