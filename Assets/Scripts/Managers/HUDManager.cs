using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
    public Inventory inventory;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (inventory != null) 
        { 
            inventory.ItemAdded += InventoryScript_ItemAdded;
        }
    }

    private void InventoryScript_ItemAdded(object sender, InventoryEventArgs e)
    {
        Transform inventoryPanel = transform.Find("Inventario");
        foreach (Transform slot in inventoryPanel)
        {
            Image image = slot.GetChild(0).GetComponent<Image>();
            if (image.enabled == false)
            {
                image.enabled = true;
                image.sprite = e.Item.Image;
                break;
            }
        }
    }
}
