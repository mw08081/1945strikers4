using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InSertCoinBlinking : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI blinkingTxt;
    
    [SerializeField] Color originFace;
    [SerializeField] Color originOutline;
    [SerializeField] Color effectFace;
    [SerializeField] Color effectOutline;
    
    [SerializeField] float onStayTime;
    [SerializeField] float offStayTime;
    [SerializeField] float delayTime;
    [SerializeField] bool isOn;

    void Start()
    {
        blinkingTxt.faceColor = originFace;
        blinkingTxt.outlineColor = originOutline;

        StartCoroutine("Blinking");
    }

    IEnumerator Blinking()
    {
        yield return new WaitForSeconds(delayTime);

        while (true)
        {
            if (isOn)
            {
                blinkingTxt.faceColor = effectFace;
                blinkingTxt.outlineColor = effectOutline;
                
                isOn = false;
                yield return new WaitForSeconds(offStayTime);
            }
            else
            {
                blinkingTxt.faceColor = originFace;
                blinkingTxt.outlineColor = originOutline;

                isOn = true;
                yield return new WaitForSeconds(onStayTime);
            }
        }
    }
}
