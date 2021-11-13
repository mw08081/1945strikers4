using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PowerTMP : MonoBehaviour
{
    [SerializeField] TextMeshPro powerUpTmp;
    [SerializeField] float speed;

    Color originColor;
    Color blueColor;

    private void Awake()
    {
        originColor = powerUpTmp.faceColor;
        blueColor = new Color(0, 0, 255);
    }

    private void OnEnable()
    {
        StartCoroutine("TmpEffect");
    }

    IEnumerator TmpEffect()
    {
        for (int i = 0; i < 10; i++)
        {
            if(powerUpTmp.faceColor.r == 0)
                powerUpTmp.faceColor = originColor;
            else
                powerUpTmp.faceColor = blueColor;

            transform.position += Vector3.forward * speed * Time.deltaTime;

            yield return new WaitForSeconds(0.1f);
        }
        powerUpTmp.faceColor = originColor;
        SystemManager.Instance.TmpSystem.ReturnPowerUpTmp(powerUpTmp);
    }
}
