
#if UNITY_EDITOR
using UnityEditor;
#endif
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
//using UnityEngine.TestTools.Constraints;

namespace Inventory.Model{
    [CreateAssetMenu]
public class Inventory_SO : ScriptableObject
{
    [SerializeField]
    private List<InventoryItem> inventoryItems;
    [SerializeField]
    private int size=10;
    public int Size { get => size; set => size = value; }
    public event Action<Dictionary<int, InventoryItem>> OnInventoryUpdated;
    public void Initialize(){
        /*inventoryItems = new List<InventoryItem>();
        for(int i=0; i< size; i++){
            inventoryItems.Add(InventoryItem.GetEmptyItem());
        }*/
    }
    void OnApplicationQuit()
    {
    ResetInventory();
        }
    public void ResetInventory()
    {
        inventoryItems = new List<InventoryItem>();
        for (int i = 0; i < size; i++)
        {
            inventoryItems.Add(InventoryItem.GetEmptyItem());
        }
    }
     #if UNITY_EDITOR
    void OnDisable()
    {
        /*if (!EditorApplication.isPlayingOrWillChangePlaymode)
        {*/
            ResetInventory(); // Reset inventory when stopping Play Mode in the Editor
        //}
    }
    #endif
    public int AddItem(ItemSO item, int quantity, List<ItemParameter> itemState=null){
        if(item.IsStackable == false)
            {
                 for (int i = 0; i < inventoryItems.Count; i++)
                {
                    while(quantity > 0 && IsInventoryFull() == false)
                    {
                        quantity -= AddItemToFirstFreeSlot(item, 1, itemState);
                    }
                    InformAboutChange();
                    return quantity;
                }
            }
        quantity = AddStackablItem(item, quantity);
        InformAboutChange();
        return quantity;        
    }

        private int AddNonStackableItem(ItemSO item, int quantity)
        {
            InventoryItem newItem = new InventoryItem{
                item=item,
                quantity=quantity
            };
            
            for(int i=0; i < inventoryItems.Count; i++){
                if(inventoryItems[i].isEmpty){
                    inventoryItems[i]=newItem;
                    return quantity;
                }
            }
            return 0;
        }

        private bool IsInventoryFull()
        => inventoryItems.Where(item=> item.isEmpty).Any()==false;

        private int AddStackablItem(ItemSO item, int quantity)
        {
         for (int i = 0; i < inventoryItems.Count; i++)
         {
            if(inventoryItems[i].isEmpty)
            continue;
            if(inventoryItems[i].item.ID == item.ID){
                int amountPossibleToTake =
                    inventoryItems[i].item.MazStackSize - inventoryItems[i].quantity;
                if(quantity > amountPossibleToTake){
                    inventoryItems[i] = inventoryItems[i].ChangeQuantity(inventoryItems[i].item.MazStackSize);
                    quantity-=amountPossibleToTake;
                }
                else{
                    inventoryItems[i] = inventoryItems[i].ChangeQuantity(inventoryItems[i].quantity+quantity);
                    InformAboutChange();
                    return 0;
                }
            }
         }
         while(quantity > 0 && IsInventoryFull()==false){
            int newQuantity = Math.Clamp(quantity,0, item.MazStackSize);
            quantity-= newQuantity;
            AddItemToFirstFreeSlot(item, newQuantity);
         }
         return quantity;
        }

    private int AddItemToFirstFreeSlot(ItemSO item, int quantity, List<ItemParameter> itemState=null)
    {
        InventoryItem newItem = new InventoryItem{
            item = item,
            quantity= quantity,
            itemState = new List<ItemParameter>(itemState==null ? item.DefaultParameterList1 : itemState)
        };
        for(int i=0; i < inventoryItems.Count; i++){
                if(inventoryItems[i].isEmpty){
                    inventoryItems[i]=newItem;
                    return quantity;
                }
            }
            return 0;
    }

        public void AddItem(InventoryItem item){
        AddItem(item.item, item.quantity);
    }
    public Dictionary<int, InventoryItem> GetCurrrentInventoryState(){
        Dictionary<int, InventoryItem> returnValue = new Dictionary<int, InventoryItem>();
        for(int i=0; i< inventoryItems.Count;i++){
            if(inventoryItems[i].isEmpty){
                continue;
            }
            returnValue[i]=inventoryItems[i];
        }
        return returnValue;
    }

    public  InventoryItem GetItemAt(int itemIndex)
    {
        return inventoryItems[itemIndex];
    }

    public void SwapItems(int itemIndex1, int itemIndex2)
    {
        InventoryItem item1= inventoryItems[itemIndex1];
        inventoryItems[itemIndex1] = inventoryItems[itemIndex2];
        inventoryItems[itemIndex2] = item1;
        InformAboutChange();
    }

    private void InformAboutChange()
    {
        OnInventoryUpdated?.Invoke(GetCurrrentInventoryState());
    }

    public void RemoveItem(int itemIndex, int amount)
    {
        if(inventoryItems.Count > itemIndex){
            if(inventoryItems[itemIndex].isEmpty)
            return;
            int reminder = inventoryItems[itemIndex].quantity - amount;
            if(reminder <= 0){
                inventoryItems[itemIndex] = InventoryItem.GetEmptyItem();
            }
            else{
                inventoryItems[itemIndex]= inventoryItems[itemIndex].ChangeQuantity(reminder);
            }
            InformAboutChange();
        }
    }
    }
[Serializable]
public struct InventoryItem{
        public int quantity;
        public ItemSO item;
        public bool isEmpty => item == null;
        public List<ItemParameter> itemState;

        public InventoryItem ChangeQuantity(int newQuantity){
            return new InventoryItem{
                item = this.item,
                quantity= newQuantity,
                itemState = new List<ItemParameter>(this.itemState)
            };
        }
        public static InventoryItem GetEmptyItem()=>
            new InventoryItem
            {
                item=null,
                quantity=0,
                itemState = new List<ItemParameter>()
            };
}

}