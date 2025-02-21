using Inventory.UI;
using Inventory.Model;

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

namespace Inventory
{
    public class InventoryManager : MonoBehaviour
{
    [SerializeField] private UI_InventoryMenu uiInventory;
    [SerializeField] public Inventory_SO inventoryData;
    [SerializeField]
    private AudioClip dropClip;

    [SerializeField]
    private AudioSource audioSource;

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
        InventoryItem inventoryItem = inventoryData.GetItemAt(itemIndex);
        if(inventoryItem.isEmpty)
        return;
        IItemAction itemAction = inventoryItem.item as IItemAction;
         if(itemAction != null)
            {
                uiInventory.ShowItemAction(itemIndex);
                uiInventory.AddAction(itemAction.ActionName, () => PerformAction(itemIndex));
            }
        IDestroyableItem destroyableItem= inventoryItem.item as IDestroyableItem;
        if(destroyableItem != null){
            uiInventory.AddAction("Drop", ()=> DropItem(itemIndex, inventoryItem.quantity));
        }
    }

    private void DropItem(int itemIndex, int quantity)
    {
        inventoryData.RemoveItem(itemIndex, quantity);
        uiInventory.ResetSelection();
        audioSource.PlayOneShot(dropClip);
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
        string description = PrepareDescription(inventoryItem);
        uiInventory.UpdateDescription(itemIndex, item.ItemImage, item.name, item.Description);
    }
    private string PrepareDescription(InventoryItem inventoryItem)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(inventoryItem.item.Description);
            sb.AppendLine();
            for (int i = 0; i < inventoryItem.itemState.Count; i++)
            {
                sb.Append($"{inventoryItem.itemState[i].itemParameter.ParameterName} " +
                    $": {inventoryItem.itemState[i].value} / " +
                    $"{inventoryItem.item.DefaultParameterList1[i].value}");
                sb.AppendLine();
            }
            return sb.ToString();
        }
    public void PerformAction(int itemIndex){
        InventoryItem inventoryItem = inventoryData.GetItemAt(itemIndex);
            if (inventoryItem.isEmpty)
                return;

            IDestroyableItem destroyableItem = inventoryItem.item as IDestroyableItem;
            if (destroyableItem != null)
            {
                inventoryData.RemoveItem(itemIndex, 1);
            }

            IItemAction itemAction = inventoryItem.item as IItemAction;
            if (itemAction != null)
            {
                itemAction.PerformAction(gameObject, inventoryItem.itemState);
                audioSource.PlayOneShot(itemAction.actionSFX);
                if (inventoryData.GetItemAt(itemIndex).isEmpty)
                    uiInventory.ResetSelection();
            }
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