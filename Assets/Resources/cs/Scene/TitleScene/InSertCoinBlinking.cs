using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InSertCoinBlinking : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI blinkingTxt;
    bool isOn = true;

    void Start()
    {
        StartCoroutine("Blinking");
    }

    IEnumerator Blinking()
    {
        Color originFace = new Color(255, 255, 255, 255);
        Color originOutline = new Color(0, 0, 0, 255);

        Color effectFace = new Color(255, 255, 255, 0);
        Color effectOutline = new Color(0, 0, 0, 0);

        while (true)
        {
            if (isOn)
            {
                blinkingTxt.faceColor = effectFace;
                blinkingTxt.outlineColor = effectOutline;
                
                isOn = false;
                yield return new WaitForSeconds(0.5f);
            }
            else
            {
                blinkingTxt.faceColor = originFace;
                blinkingTxt.outlineColor = originOutline;

                isOn = true;
                yield return new WaitForSeconds(1f);
            }
        }
    }
}
