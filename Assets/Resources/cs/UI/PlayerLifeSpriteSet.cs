using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerLifeSpriteSet : MonoBehaviour
{
    [SerializeField] Image[] lifes;
    string[] paths = new string[] { "UI_Img/BF109", "UI_Img/P38", "UI_Img/FlyingPancake" };

    bool isP1;

    void Start()
    {
        if (SystemManager.Instance.GetCurrentSceneT<Stage1Scene>().Player2 == null)
            isP1 = true;
        else
            isP1 = false;

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
}
