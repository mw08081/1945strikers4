using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerLifeSpriteSet : MonoBehaviour
{
    [SerializeField] bool isP1;

    [SerializeField] Image[] lifes;
    string[] paths = new string[] { "UI_Img/BF109", "UI_Img/P38", "UI_Img/FlyingPancake" };

    void Start()
    {
        if(SystemManager.Instance.isForDos)
        {
            if (isP1)
            {
                for (int i = 0; i < lifes.Length; i++)
                    lifes[i].sprite = Resources.Load<Sprite>(paths[SystemManager.Instance.Player1PrefabIndex]);
            }
            else
            {
                for (int i = 0; i < lifes.Length; i++)
                    lifes[i].sprite = Resources.Load<Sprite>(paths[SystemManager.Instance.Player2PrefabIndex]);
            }
        }
        else
        {
            if(isP1)
            {
                for (int i = 0; i < lifes.Length; i++)
                    lifes[i].sprite = Resources.Load<Sprite>(paths[SystemManager.Instance.Player1PrefabIndex]);
            }
            else
            {
                for (int i = 0; i < lifes.Length; i++)
                    lifes[i].gameObject.SetActive(false);
            }
            
        }
        
            
    }
}
