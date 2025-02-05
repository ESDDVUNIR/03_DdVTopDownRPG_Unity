using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_InventoryMenu : MonoBehaviour
{
    [SerializeField] private Ui_Item itemPrefab;
    [SerializeField] private RectTransform contentPanel;
    [SerializeField] private UI_InventoryDescription itemDescription; 
    [SerializeField] private MouseFollower mouseFollower;

    List<Ui_Item> listofUIItems = new List<Ui_Item>();
    public event Action<int> OnDescriptionRequested, OnItemActionRequested, OnStartDragging;
    public event Action<int,int> OnSwapItems;
    private int currentlyDraggedItem= -1;

    private void Awake(){
        itemDescription.ResetDescription();
        mouseFollower.Toggle(false);
    }

    public void InitializeInventory(int invMenusize){
        for(int i=0 ; i<invMenusize;i++){
            Ui_Item uiItem = Instantiate(itemPrefab, Vector3.zero, Quaternion.identity);
            uiItem.transform.SetParent(contentPanel);
            listofUIItems.Add(uiItem);
            //Clicking and Dragging Options
            uiItem.OnItemClicked += HandleItemSelection;
            uiItem.OnItemBeginDrag += HandleBeginDrag;
            uiItem.OnItemDroppedOn += HandleSwap;
            uiItem.OnItemEndDrag += HandleEndDrag;
            uiItem.OnRightMouseBtnClick += HandleShowItemActions;
        }
    }
    public void Updatedata(int itemIndex, Sprite itemImage, int itemQuantity){
        if(listofUIItems.Count > itemIndex){
            listofUIItems[itemIndex].SetData(itemImage, itemQuantity);
        }
    }
    private void HandleShowItemActions(Ui_Item item)
    {

    }

    private void HandleEndDrag(Ui_Item item)
    {
            ResetDraggedItem();
    }

    private void HandleSwap(Ui_Item item)
    {
        Debug.Log("SWAPING NOW!");
        int index = listofUIItems.IndexOf(item);
        if(index==-1){
            return;
        }
        OnSwapItems?.Invoke(currentlyDraggedItem, index);
    }

    private void ResetDraggedItem()
    {
       mouseFollower.Toggle(false);
       currentlyDraggedItem = -1;
    }

    private void HandleBeginDrag(Ui_Item item)
    {
        Debug.Log("BEGIN DRAG!");
        int index = listofUIItems.IndexOf(item);
        if(index==-1){
            return;
        }
        currentlyDraggedItem = index;
        HandleItemSelection(item);
        OnStartDragging?.Invoke(index);
    }
    private void CreateDraggedItem(Sprite sprite, int quantity){
        mouseFollower.Toggle(true);
        mouseFollower.SetData(sprite, quantity);
    }

    private void HandleItemSelection(Ui_Item item)
    {
        int index = listofUIItems.IndexOf(item);
        if(index==-1) return;
        OnDescriptionRequested?.Invoke(index);
    }
    public void Show(){
        ResetSelection();
    }
    public void Hide(){
        itemDescription.ResetDescription();
            ResetDraggedItem();
        gameObject.SetActive(false);
    }

    public void ResetSelection()
    {
        gameObject.SetActive(true);
        itemDescription.ResetDescription();
        DeselectAllItems();
    }

    private void DeselectAllItems()
    {
        foreach(Ui_Item item in listofUIItems){
            item.Deselect();
        }
    }

    internal void UpdateDescription(int itemIndex, Sprite image, string name, string description)
    {
        itemDescription.SetDescription(image, name, description);
        DeselectAllItems(); 
        listofUIItems[itemIndex].Select();
    }
}
