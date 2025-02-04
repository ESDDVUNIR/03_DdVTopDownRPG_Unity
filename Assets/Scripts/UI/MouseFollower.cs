using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseFollower : MonoBehaviour
{
    [SerializeField] private Canvas canvas;
    [SerializeField] private Ui_Item item;

    void Awake(){
        canvas= transform.root.GetComponent<Canvas>();
        item= GetComponentInChildren<Ui_Item>();
        Toggle(false);
    }
    public void SetData(Sprite sprite, int quantity){
        item.SetData(sprite, quantity);
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
