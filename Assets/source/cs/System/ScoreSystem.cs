using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreSystem
{
    public int Score { get; set; }

    public void CalcSc(int val)
    {
        Score += val;
    }

}
