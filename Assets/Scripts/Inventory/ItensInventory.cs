using UnityEngine;
using System;

public interface InventoryItem { 
    string Name { get; }
    Sprite Image { get; }
    void OnPickup();
}

public class InventoryEventArgs : EventArgs 
{
    public InventoryEventArgs(InventoryItem item)
    {
        Item = item;
    }
    public InventoryItem Item;
}
