using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class F5UBomb : MonoBehaviour
{
    enum Status : int
    {
        Appear = 0,
        Stay,
        Out,
    }
    [Header("----F5UBomber Status----")]
    [SerializeField] Status state;
    BulletCode bulletCode;

    [Header("----F5UBomber Status.AppearInfo----")]
    [SerializeField] Vector3 appearDisPos;
    float playerPosZ;

    [Header("----F5UBomber Status.StayInfo----")]
    [SerializeField] float speed;
    [SerializeField] float stayTime;
    [SerializeField] float lifeTime;
    public bool isAttackAdmission;

    [Header("----F5UBomber Status.OutInfo----")]
    [SerializeField] Vector3 outDisPos;


    private void OnEnable()
    {
        state = Status.Appear;

        lifeTime = stayTime;
        isAttackAdmission = false;

        playerPosZ = SystemManager.Instance.GetCurrentSceneT<InGameScene>().Player.transform.position.z + 2;
        appearDisPos = new Vector3(transform.position.x, -3, playerPosZ);
        outDisPos = new Vector3(transform.position.x, -3, 22);
    }

    void Update()
    {
        UpdateMove();
    }

    void UpdateMove()
    {
        switch (state)
        {
            case Status.Appear:
                UpdateMoveStateAppear();
                break;
            case Status.Stay:
                UpdateMoveStateStay();
                break;
            case Status.Out:
                UpdateMoveStateOut();
                break;
            default:
                break;
        }
    }

    void UpdateMoveStateAppear()
    {
        Vector3 refV = Vector3.zero;
        transform.position = Vector3.SmoothDamp(transform.position, appearDisPos, ref refV, 0.1f);

        if (Vector3.Distance(transform.position, appearDisPos) < 0.1f)
        {
            state = Status.Stay;
            isAttackAdmission = true;
        }
    }
    void UpdateMoveStateStay()
    {
        lifeTime -= Time.deltaTime;
        transform.position += Vector3.forward * speed * Time.deltaTime;

        if (lifeTime < 0 || transform.position.z > 21f)
        {
            state = Status.Out;
            isAttackAdmission = false;
        }
            
    }
    void UpdateMoveStateOut()
    {
        Vector3 refV = Vector3.zero;
        transform.position = Vector3.SmoothDamp(transform.position, outDisPos, ref refV, 0.1f);

        if (Vector3.Distance(transform.position, outDisPos) < 0.1f)
        {
            gameObject.SetActive(false);
            SystemManager.Instance.GetCurrentSceneT<InGameScene>().BulletSystem.ReturnBullet(bulletCode, gameObject);
        }
    }

    public void SetAppearDistPos(BulletCode _bulletCode, float _x)
    {
        bulletCode = _bulletCode;
        appearDisPos.x = _x;
        outDisPos.x = _x;
    }
}
