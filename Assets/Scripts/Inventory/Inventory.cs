using UnityEngine;
using System;
using System.Collections.Generic;

public class Inventory : MonoBehaviour
{
    private const int slots = 4;

    private List<InventoryItem> mItens = new List<InventoryItem>();

    public event EventHandler<InventoryEventArgs> ItemAdded;

    public void AddItem(InventoryItem item)
    {
        if (mItens.Count < slots)
        {
            Collider collider = (item as MonoBehaviour).GetComponent<Collider>();
            if (collider.enabled)
            {
                collider.enabled = false;
                mItens.Add(item);
                item.OnPickup();

                if (ItemAdded != null)
                {
                    ItemAdded(this, new InventoryEventArgs(item));
                }
            }
        }
    }
}
