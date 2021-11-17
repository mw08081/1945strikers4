using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerChoiceImg : MonoBehaviour
{
    [SerializeField] Text[] playerChoiceTxt;
    string[, ] playerChoiceTxtCont = new string[,] 
    { 
        { "BF-109 MesserSchmitt", "P-38 LIGHTNING", "F5U - FlyingPancake" },
        { "1", "1", "1" },
        { "9.02m", "11.53m", "11.1m" },
        { "9.93m", "15.85m", "8.72m" },
        { "1475hp/2600rpm", "1600hp/3100rpm", "1600hp/3100rpm" },
        { "640km/h/6000H", "666km/h/7620H", "806km/h/5850H" },
        { "2803kg", "7435kg", "4530kg" }
    };

    [SerializeField] Image playerChoiceImg;
    [SerializeField] Image playerChoiceImg2;
    [SerializeField] Sprite[] playerChoiceSpritList;
    [SerializeField] Sprite[] playerChoiceSprit2List;

    int index;
    int i;
    
    private void Start()
    {
        index = 0;
    }

    public void UpdatePlayerChoice(int _index)
    {
        index = _index;
        //StartCoroutine("PrintingInfo");
        for (i = 0; i < 7; i++)
            StartCoroutine("PrintInfo");

        playerChoiceImg.sprite = playerChoiceSpritList[index];
        playerChoiceImg2.sprite = playerChoiceSprit2List[index];
    }

    /*
    IEnumerator PrintingInfo()
    {
        int i = 0;
        while(i < 7)
        {
            int substirngCnt = 0;

            while(substirngCnt < playerChoiceTxtCont[i, index].Length)
            {
                playerChoiceTxt[i].text = playerChoiceTxtCont[i, index].Substring(0, substirngCnt + 1);

                substirngCnt++;

                yield return new WaitForSeconds(0.03f);
            }
            i++;
            yield return new WaitForSeconds(0.001f);
        }
    }
    */
    IEnumerator PrintInfo()
    {
        int substirngCnt = 0;
        int tmp = i;

        while (substirngCnt < playerChoiceTxtCont[tmp, index].Length)
        {
            playerChoiceTxt[tmp].text = playerChoiceTxtCont[tmp, index].Substring(0, substirngCnt + 1);

            substirngCnt++;

            yield return new WaitForSeconds(0.03f);
        }
    }
}
