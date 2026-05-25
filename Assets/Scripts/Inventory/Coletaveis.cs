using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Coletaveis : MonoBehaviour, InventoryItem
{
    public string Name
    {
        get { return "Coletaveis"; }
    }

    public Sprite _Image = null;

    public Sprite Image
    {
        get { return _Image; }
    }

    public void OnPickup()
    {
        gameObject.SetActive(false);
    }
}
