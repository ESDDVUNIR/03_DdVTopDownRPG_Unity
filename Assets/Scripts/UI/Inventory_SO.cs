using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Inventory_SO : ScriptableObject
{
    [SerializeField]
    private List<InventoryItem> inventoryItems;
    [SerializeField]
    private int size=10;
    public int Size { get => size; set => size = value; }
    public void Initialize(){
        inventoryItems = new List<InventoryItem>();
        for(int i=0; i< size; i++){
            inventoryItems.Add(InventoryItem.GetEmptyItem());
        }
    }
    public void AddItem(ItemSO item, int quantity){
        for(int i=0; i< inventoryItems.Count;i++){
            if(inventoryItems[i].isEmpty){
                inventoryItems[i] = new InventoryItem{
                    item = item,
                    quantity=quantity,
                };
            }
        }
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
}
[Serializable]
public struct InventoryItem{
        public int quantity;
        public ItemSO item;
        public bool isEmpty => item == null;

        public InventoryItem ChangeQuantity(int newQuantity){
            return new InventoryItem{
                item = this.item,
                quantity= newQuantity,
            };
        }
        public static InventoryItem GetEmptyItem()=>
            new InventoryItem
            {
                item=null,
                quantity=0
            };
}
