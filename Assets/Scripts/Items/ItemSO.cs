using System;
using System.Collections.Generic;
using UnityEngine;

namespace Inventory.Model{
    public abstract class ItemSO : ScriptableObject
    {
        private int myPro;
       [field: SerializeField]
       private bool isStackable;
       public int ID => GetInstanceID();

       [field: SerializeField]
       private int maxStackSize=1;
       [field: SerializeField]
       private string name;
       [field: SerializeField]
       [field: TextArea]
       private string description;
       [field: SerializeField]
       private Sprite itemImage;
       [field: SerializeField]
       private List<ItemParameter> DefaultParameterList;
    
        public bool IsStackable { get => isStackable; set => isStackable = value; }
        public int MazStackSize { get => maxStackSize; set => maxStackSize = value; }
        public string Name { get => name; set => name = value; }
        public string Description { get => description; set => description = value; }
        public int MyPro { get => myPro; set => myPro = value; }
        public Sprite ItemImage { get => itemImage; set => itemImage = value; }
        public List<ItemParameter> DefaultParameterList1 { get => DefaultParameterList; set => DefaultParameterList = value; }
    }
    [Serializable]
    public struct ItemParameter : IEquatable<ItemParameter>
    {
        public ItemParameterSO itemParameter;
        public float value;
        public bool Equals(ItemParameter other){
            return other.itemParameter == itemParameter;
        }
    }
}