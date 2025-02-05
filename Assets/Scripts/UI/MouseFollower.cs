using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseFollower : MonoBehaviour
{
    [SerializeField] private Canvas canvas;
    [SerializeField] private Ui_Item item;
    private Ui_Item showItem;

    void Awake(){
        
    }
    void Start(){
        canvas= transform.root.GetComponent<Canvas>();
        //showItem= GetComponentInChildren<Ui_Item>();
        
        //Toggle(false);
    }
    public void SetData(Sprite sprite, int quantity){

        item.SetData(sprite, quantity);
        //showItem.SetData(sprite, quantity);

    }
    void Update(){
        Vector2 position;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            (RectTransform)canvas.transform,
            Input.mousePosition,
            canvas.worldCamera,
            out position
        );
        transform.position = canvas.transform.TransformPoint(position);
    }
    public void Toggle(bool val){
        gameObject.SetActive(val);

    }
}
