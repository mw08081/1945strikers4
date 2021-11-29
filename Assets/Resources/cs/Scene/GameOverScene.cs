using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GameOverScene : MonoBehaviour
{
    [SerializeField] Text scTxt;

    // Start is called before the first frame update
    void Start()
    {
        //scTxt.text = SystemManager.Instance.ScoreSystem.Score.ToString();
        Debug.Log(SystemManager.Instance.ScoreSystem.Player1Score + " " + SystemManager.Instance.ScoreSystem.Player2Score);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
