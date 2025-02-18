using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public GameObject InventoryMenu;
    private bool menuActivated;
    public ItemSlot[] itemSlots;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // if key "C" is pressed, show inventory menu
        if(Input.GetKeyDown(KeyCode.C)){
            if(menuActivated == false){
                InventoryMenu.SetActive(true);
                menuActivated = true;
                // pause the game
                Time.timeScale = 0;
            } else{
                InventoryMenu.SetActive(false);
                menuActivated = false;
                // resume the game
                Time.timeScale = 1;
            }
        }
        
    }

    public void AddItem(string itemName, int quantity, Sprite itemSprite)
    {
        for(int i = 0; i < itemSlots.Length; i++){
            if(itemSlots[i].isFull == false){
                itemSlots[i].AddItem(itemName, quantity, itemSprite);
                return;
            }
        }
        
    }
}
