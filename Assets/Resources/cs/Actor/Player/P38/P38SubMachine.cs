using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P38SubMachine : MonoBehaviour
{
    enum Status : int
    {
        Appear = 0,
        Flip,
        Attack,
        Out,
    }

    [SerializeField] Status status;
    [SerializeField] float speed;
    Vector3 originPos;
    Vector3 attackPos;

    float lastBombDropTime;

    private void OnEnable()
    {
        lastBombDropTime = 0;
    }

    void Update()
    {
        UpdateMove();
    }

    void UpdateMove()
    {
        switch (status)
        {
            case Status.Appear:
                UpdateMoveStatusAppear();
                break;
            case Status.Flip:
                UpdateMoveStatusFlip();
                break;
            case Status.Attack:
                UpdateMoveStatusAttack();
                break;
            case Status.Out:
                UpdateMoveStatusOut();
                break;
            default:
                break;
        }
    }

    void UpdateMoveStatusAppear()
    {
        transform.position += Vector3.forward * speed * Time.deltaTime;

        if(Time.time - lastBombDropTime > 0.5f)
        {
            SystemManager.Instance.GetCurrentSceneT<Stage1Scene>().BulletSystem.ServeBullet(BulletCode.player1SubBullet2, transform.position);
            lastBombDropTime = Time.time;
        }
        

        if (Vector3.Distance(transform.position, originPos) > 25.0f)
            status = Status.Flip;
    }
    void UpdateMoveStatusFlip()
    {

    }
    void UpdateMoveStatusAttack()
    {

    }
    void UpdateMoveStatusOut()
    {

    }
    public void SettingPos(Vector3 _originPos, Vector3 _attackPos)
    {
        originPos = _originPos;
        attackPos = _attackPos;
    }
}
