using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreSystem
{
    public int Player1Score { get; set; }
    public int Player2Score { get; set; }

    public void CalcSc(bool isP1, int val)
    {
        if (isP1)
            Player1Score += val;
        else
            Player2Score += val;
    }

}
