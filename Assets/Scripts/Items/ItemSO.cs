using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ItemSO : ScriptableObject
{
    private int myPro;
   [field: SerializeField]
   private bool isStackable;
   private int ID => GetInstanceID();

   [field: SerializeField]
   private int maxStackSize=1;
   [field: SerializeField]
   private string name;
   [field: SerializeField]
   [field: TextArea]
   private string description;
   [field: SerializeField]
   private Sprite itemImage;

    public bool IsStackable { get => isStackable; set => isStackable = value; }
    public int MazStackSize { get => maxStackSize; set => maxStackSize = value; }
    public string Name { get => name; set => name = value; }
    public string Description { get => description; set => description = value; }
    public int MyPro { get => myPro; set => myPro = value; }
    public Sprite ItemImage { get => itemImage; set => itemImage = value; }
}
