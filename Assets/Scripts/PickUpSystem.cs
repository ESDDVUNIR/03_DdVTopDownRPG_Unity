using System.Collections;
using System.Collections.Generic;
using Inventory.Model;
using UnityEngine;

public class PickUpSsystem : MonoBehaviour
{
     [SerializeField]
    private Inventory_SO inventoryData;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("COLLIDE WITH ITEN");
        Item item = collision.GetComponent<Item>();
        if (item != null)
        {
            int reminder = inventoryData.AddItem(item.InventoryItem, item.Quantity);
            if (reminder == 0){       
                item.DestroyItem();
                Debug.Log("not reminder");
                }
            else
                item.Quantity = reminder;
                Debug.Log("Reminder");
        }
    }
}
