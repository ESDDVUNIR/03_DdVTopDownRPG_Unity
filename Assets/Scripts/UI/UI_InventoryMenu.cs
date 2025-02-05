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
    public Sprite theSprite, theSprite2;
    public int quantity;
    public string title;
    public string theDescription;
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
            Debug.Log($"Subscribing {uiItem.name} to OnItemDroppedOn");
            uiItem.OnItemDroppedOn += HandleSwap;
            uiItem.OnItemEndDrag += HandleEndDrag;
            uiItem.OnRightMouseBtnClick += HandleShowItemActions;
        }
    }

    private void HandleShowItemActions(Ui_Item item)
    {

    }

    private void HandleEndDrag(Ui_Item item)
    {
        Debug.Log("END DRAG!");
        mouseFollower.Toggle(false);
    }

    private void HandleSwap(Ui_Item item)
    {
        Debug.Log("SWAPING NOW!");
        int index = listofUIItems.IndexOf(item);
        if(index==-1){
            mouseFollower.Toggle(false);
            currentlyDraggedItem = -1;
            return;
        }
        // Swap item data
        Sprite tempSprite = listofUIItems[currentlyDraggedItem].ItemImage.sprite;
        int tempQuantity = int.Parse(listofUIItems[currentlyDraggedItem].QuantityTxt.text);

        listofUIItems[currentlyDraggedItem].SetData(item.ItemImage.sprite, int.Parse(item.QuantityTxt.text));
        listofUIItems[index].SetData(tempSprite, tempQuantity);

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
        mouseFollower.Toggle(true);
        mouseFollower.SetData(index ==0 ? theSprite : theSprite2, quantity);
    }

    private void HandleItemSelection(Ui_Item item)
    {
        Debug.Log("ITEM SELECTION!");
        itemDescription.SetDescription(theSprite, title, theDescription); 
        listofUIItems[0].Select();
    }

    public void ShowHide(){
        
        
        gameObject.SetActive(!gameObject.activeSelf);
        if(gameObject.activeSelf){
            listofUIItems[0].SetData(theSprite, quantity);
            listofUIItems[1].SetData(theSprite2, quantity);

        }
        else{
            itemDescription.ResetDescription();
        }
    }
    

}
