using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{
    // ===== Item Data =====
    public string itemName;
    public int quantity;
    public Sprite itemSprite;
    public bool isFull;
    // ===== Item Slot =====
    [SerializeField] TMP_Text quantity_text;
    [SerializeField] Image item_image;
    
    public void AddItem(string itemName, int quantity, Sprite itemSprite){
        this.itemName = itemName;
        this.quantity = quantity;
        this.itemSprite = itemSprite;
        item_image.sprite = itemSprite;
        quantity_text.text = quantity.ToString();
        isFull = true;
        quantity_text.enabled = true;
    }
}
