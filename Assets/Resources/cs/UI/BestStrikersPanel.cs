using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BestStrikersPanel : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI[] textMeshProUGUIs;
    
    string[] playerModel = new string[]
    {
        "BF109", "P38", "F5U"
    };
    string[,] keyArr = new string[,]
    {
        {"1thName","2thName","3thName" ,"4thName","5thName"},
        {"1thModel","2thModel","3thModel" ,"4thModel","5thModel"},
        {"1thScore","2thScore","3thScore" ,"4thScore","5thScore"}
    };

    char[] playerNameTmp = new char[3] { '-', '-', '-' };

    int P1UpdateIndex;
    int P2UpdateIndex;
    bool isP1Update;

    int tmpIndex;

    void Start()
    {
        P1UpdateIndex = -1;
        P2UpdateIndex = -1;

        tmpIndex = 0;

        SaveData();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F12))
            ResetData();

        if(P1UpdateIndex != -1)
            UpdatePlayerName();

        if (isP1Update && P2UpdateIndex != -1)
            UpdatePlayerName();
        
    }

    void UpdatePlayerName()
    {
        Debug.Log(tmpIndex);

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (playerNameTmp[tmpIndex] == '-')
                playerNameTmp[tmpIndex] = 'A';
            else if(playerNameTmp[tmpIndex] < 90)
                playerNameTmp[tmpIndex]++;
        }
        else if(Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (playerNameTmp[tmpIndex] == '-')
                playerNameTmp[tmpIndex] = 'Z';
            else if(playerNameTmp[tmpIndex] > 65)
                playerNameTmp[tmpIndex]--;
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            tmpIndex++;

            if (tmpIndex == 3)
            {
                isP1Update = true;
                PlayerPrefs.SetString(keyArr[0, P1UpdateIndex], textMeshProUGUIs[P1UpdateIndex * 2].text);
                
                playerNameTmp = new char[3] { '-', '-', '-' };
                P1UpdateIndex = P2UpdateIndex;
                tmpIndex = 0;
            }
        }
        textMeshProUGUIs[P1UpdateIndex * 2].text = playerNameTmp[0] + " " + playerNameTmp[1] + " " + playerNameTmp[2];
        textMeshProUGUIs[P1UpdateIndex * 2 + 1].text = playerNameTmp[0] + " " + playerNameTmp[1] + " " + playerNameTmp[2];
    }


    public void SaveData()
    {
        int player1ScoreTmp = SystemManager.Instance.ScoreSystem.Player1Score;
        if (PlayerPrefs.GetInt("5thScore") <= player1ScoreTmp)
        {
            for (int i = 0; i < 5; i++)
            {
                if (PlayerPrefs.GetInt(keyArr[2, i]) <= player1ScoreTmp)
                {
                    for (int j = 3; j >= i; j--)
                    {
                        PlayerPrefs.SetString(keyArr[0, j + 1], PlayerPrefs.GetString(keyArr[0, j]));
                        PlayerPrefs.SetString(keyArr[1, j + 1], PlayerPrefs.GetString(keyArr[1, j]));
                        PlayerPrefs.SetInt(keyArr[2, j + 1], PlayerPrefs.GetInt(keyArr[2, j]));
                    }
                    PlayerPrefs.SetInt(keyArr[2, i], player1ScoreTmp);
                    PlayerPrefs.SetString(keyArr[1, i], playerModel[SystemManager.Instance.Player1PrefabIndex]);

                    P1UpdateIndex = i;
                    break;
                }
            }
        }


        if (SystemManager.Instance.isForDos && (PlayerPrefs.GetInt("5thScore") <= SystemManager.Instance.ScoreSystem.Player2Score))
        {
            int player2ScoreTmp = SystemManager.Instance.ScoreSystem.Player2Score;

            for (int i = 0; i < 5; i++)
            {
                if (PlayerPrefs.GetInt(keyArr[2, i]) <= player2ScoreTmp)
                {
                    for (int j = 3; j >= i; j--)
                    {
                        PlayerPrefs.SetInt(keyArr[2, j + 1], PlayerPrefs.GetInt(keyArr[2, j]));
                        PlayerPrefs.SetString(keyArr[1, j + 1], PlayerPrefs.GetString(keyArr[1, j]));
                    }
                    PlayerPrefs.SetInt(keyArr[2, i], player2ScoreTmp);
                    PlayerPrefs.SetString(keyArr[1, i], playerModel[SystemManager.Instance.Player2PrefabIndex]);

                    P2UpdateIndex = i;
                    if (player2ScoreTmp >= player1ScoreTmp)
                        P1UpdateIndex++;

                    break;
                }
            }
        }

        PrintData();
    }
    void PrintData()
    {
        int idx = 0;
        for (int i = 0; i < keyArr.GetLength(0); i++)
        {
            for (int j = 0; j < keyArr.GetLength(1); j++)
            {
                if (i < 2)
                {
                    textMeshProUGUIs[idx].text = PlayerPrefs.GetString(keyArr[i, j]);
                    textMeshProUGUIs[idx + 1].text = PlayerPrefs.GetString(keyArr[i, j]);
                }
                else
                {
                    textMeshProUGUIs[idx].text = PlayerPrefs.GetInt(keyArr[i, j]).ToString();
                    textMeshProUGUIs[idx + 1].text = PlayerPrefs.GetInt(keyArr[i, j]).ToString();
                }

                idx += 2;
            }
        }
    }


    void ResetData()
    {
        for (int i = 0; i < 5; i++)
            PlayerPrefs.SetString(keyArr[0, i], "- - -");
        for (int i = 0; i < 5; i++)
            PlayerPrefs.SetString(keyArr[1, i], "- - -");
        for (int i = 0; i < 5; i++)
            PlayerPrefs.SetInt(keyArr[2, i], 0);

        Debug.Log("RESET DATA!!!");
        PrintData();
    }
}
