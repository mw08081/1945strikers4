using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerLifeSpriteSet : MonoBehaviour
{
    [SerializeField] Image[] lifes;
    string[] paths = new string[] { "UI_Img/Bf109", "UI_Img/p-38", "UI_Img/FlyingPancake" };

    void Start()
    {
        for (int i = 0; i < lifes.Length; i++)
            lifes[i].sprite = Resources.Load<Sprite>(paths[SystemManager.Instance.PlayerPrefabIndex]);
    }
}
