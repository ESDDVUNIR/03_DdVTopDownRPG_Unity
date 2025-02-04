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
    public Sprite theSprite;
    public int quantity;
    public string title;
    public string theDescription;

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

    private void HandleShowItemActions(Ui_Item item)
    {
    }

    private void HandleEndDrag(Ui_Item item)
    {
        mouseFollower.Toggle(false);
    }

    private void HandleSwap(Ui_Item item)
    {
        
    }

    private void HandleBeginDrag(Ui_Item item)
    {
        mouseFollower.Toggle(true);
        mouseFollower.SetData(theSprite, quantity);
    }

    private void HandleItemSelection(Ui_Item item)
    {
        itemDescription.SetDescription(theSprite, title, theDescription); 
        listofUIItems[0].Select();
    }

    public void ShowHide(){
        
        
        gameObject.SetActive(!gameObject.activeSelf);
        if(gameObject.activeSelf){
            listofUIItems[0].SetData(theSprite, quantity);
        }
        else{
            itemDescription.ResetDescription();
        }
    }
    

}
