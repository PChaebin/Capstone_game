using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] private string itemName;
    [SerializeField] private int quantity;
    [SerializeField] private GameObject _gameObject;

    private InventoryManager invectoryManager;

    // Start is called before the first frame update
    void Start()
    {
        invectoryManager = GameObject.Find("InventoryCanvas").GetComponent<InventoryManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("�浹����");
        if(collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("�÷��̾�� �浹����");
            invectoryManager.AddItem(itemName, quantity, _gameObject);
            Destroy(gameObject);
            Debug.Log("������ ����");
        }
    }
}
