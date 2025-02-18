using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] private string itemName;
    [SerializeField] private int quantity;
    [SerializeField] private Sprite itemSprite;

    private InventoryManager inventoryManager;
    // Start is called before the first frame update
    void Start()
    {
        inventoryManager = GameObject.Find("InventoryCanvas").GetComponent<InventoryManager>();
    }

    // Update is called once per frame
    void Update()
    {
        // if player is near item, add item to inventory
        if(Vector3.Distance(GameObject.Find("Player").transform.position, transform.position) <= 0.5f){
            if(Input.GetKeyDown(KeyCode.Z)){
                inventoryManager.AddItem(itemName, quantity, itemSprite);
                Destroy(gameObject);
            }
        }
    }

}
