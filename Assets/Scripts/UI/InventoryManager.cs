using Inventory.UI;
using Inventory.Model;

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Inventory
{
    public class InventoryManager : MonoBehaviour
{
    [SerializeField] private UI_InventoryMenu uiInventory;
    [SerializeField] public Inventory_SO inventoryData;

    public List<InventoryItem> initialItems = new List<InventoryItem>();

        public List<InventoryItem> InitialItems { get => initialItems; }

        void Awake(){
        PrepareUI();
        uiInventory.Hide();
        PrepareInventoryData();
    }

        private void PrepareInventoryData()
        {
            inventoryData.Initialize();
            inventoryData.OnInventoryUpdated += UpdateInventoryUI;
            foreach (InventoryItem item in initialItems)
            {
                if (item.isEmpty) continue;
                inventoryData.AddItem(item);
            }
        }

        private void UpdateInventoryUI(Dictionary<int, InventoryItem> inventoryState)
        {
            uiInventory.ResetAllItems();
            foreach (var item in inventoryState)
            {
                uiInventory.Updatedata(item.Key, item.Value.item.ItemImage, item.Value.quantity);

            }
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
        InventoryItem inventoryItem = inventoryData.GetItemAt(itemIndex);
        if(inventoryItem.isEmpty)
        return;
        uiInventory.CreateDraggedItem(inventoryItem.item.ItemImage, inventoryItem.quantity);
    }

    private void HandleSwapItems(int itemIndex1, int itemIndex2)
    {
        inventoryData.SwapItems(itemIndex1, itemIndex2);
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
}