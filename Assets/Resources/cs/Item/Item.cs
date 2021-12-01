using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] public ItemCode itemCode;
    [SerializeField] protected Vector3 moveDir;
    [SerializeField] float speed;

    void Start()
    {
        
    }

    private void OnEnable()
    {
        Initializing();
    }

    protected virtual void Initializing()
    {
        moveDir = new Vector3(Random.Range(-1.0f, 1.0f), 0, Random.Range(-1.0f, 1.0f));
    }

    void Update()
    {
        UpdatingMove();
        CheckBound();
    }

    void UpdatingMove()
    {
        transform.position += moveDir.normalized * speed * Time.deltaTime;
    }

    protected virtual void CheckBound() { }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
            ReturnGameObject();
    }

    protected void ReturnGameObject()
    {
        gameObject.SetActive(false);
        SystemManager.Instance.GetCurrentSceneT<InGameScene>().ItemSystem.ReturnItem(itemCode, gameObject);
    }
}
