using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncingItem : Item
{
    public int boundingCnt = 3;
    bool isVLock = false;
    bool isHLock = false;

    protected override void Initializing()
    {
        base.Initializing();

        isVLock = false;
        isHLock = false;

        boundingCnt = 3;
    }

    protected override void CheckBound()
    {
        //base.CheckBound();
        if (boundingCnt <= 0)
            ReturnGameObject();

        Vector3 screenPoint = Camera.main.WorldToViewportPoint(transform.position);
        if (screenPoint.x <= 0.1f || screenPoint.x >= 0.9f)
        {
            if (isHLock)
            {
                if (!(screenPoint.x >= 0.1f && screenPoint.x <= 0.9f))
                    isHLock = false;

                return;
            }
            moveDir = (new Vector3(-moveDir.x, 0, moveDir.z)).normalized;
            boundingCnt--;

            isHLock = true;
        }

        if (screenPoint.y <= 0.02f || screenPoint.y >= 0.98f)
        {
            if (isVLock)
            {
                if (screenPoint.y >= 0.02f && screenPoint.y <= 0.98f)
                    isVLock = false;

                return;
            }
            moveDir = (new Vector3(moveDir.x, 0, -moveDir.z)).normalized;
            boundingCnt--;

            isVLock = true;
        }
    }
}
