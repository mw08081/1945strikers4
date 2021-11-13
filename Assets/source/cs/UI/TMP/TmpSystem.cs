using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TmpSystem : MonoBehaviour
{
    [SerializeField] TextMeshPro powerUpTmpPrefab;

    Queue<TextMeshPro> powerUpTmpQueue = new Queue<TextMeshPro>();

    void Start()
    {
        SettingTmp();
    }

    void SettingTmp()
    {
        for(int i = 0; i < 3; i++)
        {
            TextMeshPro powerUpTmp = Instantiate<TextMeshPro>(powerUpTmpPrefab, transform);
            powerUpTmp.gameObject.SetActive(false);

            powerUpTmpQueue.Enqueue(powerUpTmp);
        }
    }

    void Update()
    {
        
    }

    public void ServePowerUpTmp(Vector3 servePos)
    {
        if (powerUpTmpQueue.Count == 0)
            powerUpTmpQueue.Enqueue(Instantiate<TextMeshPro>(powerUpTmpPrefab, transform));

        TextMeshPro textMeshPro = powerUpTmpQueue.Dequeue();
        
        textMeshPro.gameObject.transform.position = servePos;
        textMeshPro.gameObject.SetActive(true);
    }

    public void ReturnPowerUpTmp(TextMeshPro textMeshPro)
    {
        textMeshPro.gameObject.SetActive(false);
        powerUpTmpQueue.Enqueue(textMeshPro);
    }
}
