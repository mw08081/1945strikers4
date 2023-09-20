using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerLifeSpriteSet : MonoBehaviour
{
    [SerializeField] bool isP1;

    [SerializeField] Image[] lifes;
    string[] paths = new string[] { "UI_Img/BF109", "UI_Img/P38", "UI_Img/FlyingPancake" };

    bool isP2OriginStatus;

    void Start()
    {
        
        if(GameManager.Instance.isForDos)
        {
            if (isP1)
            {
                for (int i = 0; i < lifes.Length; i++)
                    lifes[i].sprite = Resources.Load<Sprite>(paths[GameManager.Instance.Player1PrefabIndex]);
            }
            else
            {
                for (int i = 0; i < lifes.Length; i++)
                    lifes[i].sprite = Resources.Load<Sprite>(paths[GameManager.Instance.Player2PrefabIndex]);
            }
        }
        else
        {
            isP2OriginStatus = false;
            if (isP1)
            {
                for (int i = 0; i < lifes.Length; i++)
                    lifes[i].sprite = Resources.Load<Sprite>(paths[GameManager.Instance.Player1PrefabIndex]);
            }
            else
            {
                for (int i = 0; i < lifes.Length; i++)
                    lifes[i].gameObject.SetActive(false);
            }
        }
    }

    private void Update()
    {
        if (!isP2OriginStatus && GameManager.Instance.isForDos && !isP1)
        {
            isP2OriginStatus = true;

            for (int i = 0; i < lifes.Length; i++)
            {
                lifes[i].gameObject.SetActive(true);
                lifes[i].sprite = Resources.Load<Sprite>(paths[GameManager.Instance.Player2PrefabIndex]);
            }
                
        }
    }
}
