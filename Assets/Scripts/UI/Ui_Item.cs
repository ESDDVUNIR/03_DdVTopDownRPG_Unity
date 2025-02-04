using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Ui_Item : MonoBehaviour
{
   [SerializeField] private Image itemImage;
   [SerializeField] private Image borderImage;
   [SerializeField] private TMP_Text quantityTxt;
    public event Action<Ui_Item> OnItemClicked, OnItemDroppedOn, OnItemBeginDrag,OnItemEndDrag, OnRightMouseBtnClick;

    private bool empty=true;
    public void Awake(){
        ResetData();
        Deselect();
    }

    private void Deselect()
    {
        borderImage.enabled=false;
    }

    private void ResetData()
    {
        this.itemImage.gameObject.SetActive(false);
    }
    public void SetData(Sprite sprite, int quantity){
        this.itemImage.gameObject.SetActive(true);
        this.itemImage.sprite = sprite;
        this.quantityTxt.text = quantity+"";
        empty=false;
    }
    public void Select(){
        this.borderImage.enabled=true;
    }
    public void OnBeginDrag(){
        if(empty)
            return;
        OnItemBeginDrag?.Invoke(this);
    }
    public void OnDrop(){
        OnItemDroppedOn?.Invoke(this);
    }
    public void OnendDrag(){
        OnItemEndDrag?.Invoke(this);
    }
    public void OnPointerClick(BaseEventData data){
        if(empty)
            return;
        PointerEventData pointerData = (PointerEventData)data;
        if(pointerData.button == PointerEventData.InputButton.Right){
            OnRightMouseBtnClick?.Invoke(this);
        }
        else{
            OnItemClicked?.Invoke(this);
        }
    }
}
