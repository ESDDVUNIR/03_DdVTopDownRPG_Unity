using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Inventory.UI{

    public class Ui_Item : MonoBehaviour, IPointerClickHandler, IBeginDragHandler,IEndDragHandler, IDropHandler, IDragHandler
    {
       [SerializeField] private Image itemImage;
       [SerializeField] private Image borderImage;
       [SerializeField] private TMP_Text quantityTxt;
        public event Action<Ui_Item> OnItemClicked, OnItemDroppedOn, OnItemBeginDrag,OnItemEndDrag, OnRightMouseBtnClick;

        private bool empty=true;

        public Image ItemImage { get => itemImage; set => itemImage = value; }
        public TMP_Text QuantityTxt { get => quantityTxt; set => quantityTxt = value; }

        public void Awake(){
            ResetData();
            Deselect();
        }

        public void Deselect()
        {
            if (borderImage == null) return;  // Prevents error if the object is destroyed
            borderImage.enabled = false;
        }

        public void ResetData()
        {
           if (itemImage == null) return;  // Prevents error
            this.itemImage.gameObject.SetActive(false);
        }
        public void SetData(Sprite sprite, int quantity){
            if(itemImage!=null){
            this.itemImage.gameObject.SetActive(true);
            this.itemImage.sprite = sprite;
            this.quantityTxt.text = quantity+"";
            empty=false;
            }
        }
        public void Select(){
            this.borderImage.enabled=true;
        }
        public void OnPointerClick(PointerEventData eventData)
        {
            //PointerEventData pointerData = (PointerEventData)eventData;
            if(eventData.button == PointerEventData.InputButton.Right){
                OnRightMouseBtnClick?.Invoke(this);
            }
            else{
                OnItemClicked?.Invoke(this);
            }
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if(empty)
                return;
            OnItemBeginDrag?.Invoke(this);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            OnItemEndDrag?.Invoke(this);
        }

        public void OnDrop(PointerEventData eventData)
        {
            Debug.Log($"OnDrop executed on {gameObject.name}");
            if (OnItemDroppedOn == null)
            {
            Debug.LogError($"OnItemDroppedOn is null for {gameObject.name}!");
            return;
            }

        OnItemDroppedOn.Invoke(this);
        }

        public void OnDrag(PointerEventData eventData)
        {

        }
    }
}