using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] private UI_InventoryMenu uiInventory;
    [SerializeField] private Inventory_SO inventoryData;

    void Awake(){
        PrepareUI();
        //inventoryData.Initialize();
        uiInventory.Hide();
    }

    private void PrepareUI()
    {
        uiInventory.InitializeInventory(inventoryData.Size);
        this.uiInventory.OnDescriptionRequested += HandleDescriptionRequest;
        this.uiInventory.OnSwapItems += HandleSwapItems;
        this.uiInventory.OnStartDragging += HandleDragging;
        this.uiInventory.OnItemActionRequested += HandleItemActionRequest;
    }

    private void HandleItemActionRequest(int itemIndex)
    {
         
    }

    private void HandleDragging(int itemIndex)
    {
        
    }

    private void HandleSwapItems(int itemIndex1, int itemIndex2)
    {
        
    }

    private void HandleDescriptionRequest(int itemIndex)
    {
        InventoryItem inventoryItem = inventoryData.GetItemAt(itemIndex);
        if(inventoryItem.isEmpty){
            uiInventory.ResetSelection();
            return;
        }
        ItemSO item = inventoryItem.item;
        uiInventory.UpdateDescription(itemIndex, item.ItemImage, item.name, item.Description);
    }

    void Update() {
        if(Input.GetKeyDown(KeyCode.I)){
            if(!uiInventory.isActiveAndEnabled){
                uiInventory.Show();
                foreach(var item in inventoryData.GetCurrrentInventoryState()){
                    uiInventory.Updatedata(item.Key,
                    item.Value.item.ItemImage,
                    item.Value.quantity);
                }
            }
            else{
                uiInventory.Hide();
            }
                
        }    
    }
}
