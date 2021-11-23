using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player2Choice : MonoBehaviour
{
    [SerializeField] float movingDistance;
    public int index;
    bool isSelected;

    void Start()
    {
        index = 0;
    }

    void Update()
    {
        PlayerChoiceMove();
    }

    void PlayerChoiceMove()
    {
        if (!isSelected)
        {
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                if (index == 2)
                {
                    transform.position -= Vector3.right * 3 * movingDistance;
                    index = -1;
                }
                transform.position += Vector3.right * movingDistance;
                index++;

                SystemManager.Instance.GetCurrentSceneT<PlayerChoiceScene>().PlayerChoiceImg.UpdatePlayerChoice(index);
            }
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                if (index == 0)
                {
                    transform.position += Vector3.right * 3 * movingDistance;
                    index = 3;
                }
                transform.position -= Vector3.right * movingDistance;
                index--;

                SystemManager.Instance.GetCurrentSceneT<PlayerChoiceScene>().PlayerChoiceImg.UpdatePlayerChoice(index);
            }
        }
    }
}
