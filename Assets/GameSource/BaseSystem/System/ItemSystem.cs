using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemCode : int
{
    powerUp = 0,
    bomb,
    point
}

public class ItemSystem : MonoBehaviour
{
    [SerializeField] GameObject[] itemPrefabs;

    List<Queue<GameObject>> itemQueueList = new List<Queue<GameObject>>();
    
    void Start()
    {
        InitializingItemSystem();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
            ServeItem(ItemCode.powerUp, new Vector3(0, -3, 0));
        if (Input.GetKeyDown(KeyCode.E))
            ServeItem(ItemCode.bomb, new Vector3(0, -3, 0));
    }

    void InitializingItemSystem()
    {
        for(int i = 0; i < itemPrefabs.Length; i++)
        {
            itemQueueList.Add(new Queue<GameObject>());
            for(int j = 0; j < 5; j++)
            {
                GameObject go = Instantiate<GameObject>(itemPrefabs[i], transform);
                go.SetActive(false);

                itemQueueList[i].Enqueue(go);
            }
        }
    }

    public void ServeItem(ItemCode itemCode, Vector3 position)
    {
        if (itemQueueList[(int)itemCode].Count == 0)
            itemQueueList[(int)itemCode].Enqueue(Instantiate(itemPrefabs[(int)itemCode]));

        GameObject go = itemQueueList[(int)itemCode].Dequeue();
        go.transform.position = position;

        go.SetActive(true);
    }

    public void ReturnItem(ItemCode itemCode, GameObject gameObejct)
    {
        itemQueueList[(int)itemCode].Enqueue(gameObejct);
    }

}
