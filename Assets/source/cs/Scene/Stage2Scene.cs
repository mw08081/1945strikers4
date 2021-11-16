using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage2Scene : BaseScene
{
    protected override void Initializing()
    {
        Debug.Log(SystemManager.Instance.PlayerHp + " " + SystemManager.Instance.ScoreSystem.Score);
    }
}
