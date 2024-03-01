using System;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    public Item item;
    private SimpleInventory _inventory;
    public bool canPickup = true;

    private void Start()
    {
        _inventory = FindObjectOfType<SimpleInventory>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && canPickup)
        {
            HUD.instance.SetInfo("press [e] to pickup");
            Interactions.instance.DoInteraction += PickUp_;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            HUD.instance.SetInfo("");
            Interactions.instance.DoInteraction -= PickUp_;
        }
    }

    public void PickUp_()
    {
        if (!canPickup) return;
        canPickup = false;
        HUD.instance.SetInfo("");
        Interactions.instance.DoInteraction -= PickUp_;
        _inventory.PickUp(item);
        Destroy(this.gameObject);
    }
}