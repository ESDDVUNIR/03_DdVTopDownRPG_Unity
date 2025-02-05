using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] private UI_InventoryMenu uiInventory;
    [SerializeField] private int inventorySize;

    public int InventorySize { get => inventorySize; set => inventorySize = value; }
    void Awake(){
        uiInventory.InitializeInventory(inventorySize);
        uiInventory.ShowHide();
    }
    void Update() {
        if(Input.GetKeyDown(KeyCode.I)){
                uiInventory.ShowHide();
        }    
    }
}
