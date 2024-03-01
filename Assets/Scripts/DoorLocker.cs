using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DoorLocker : MonoBehaviour
{
    [SerializeField] Door door;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !door.isOpen && !door.opening && !door.closing)
        {
            HUD.instance.SetInfo("press [e] to open");
            Interactions.instance.DoInteraction += Open;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && !door.isOpen)
        {
            HUD.instance.SetInfo("");
            Interactions.instance.DoInteraction -= Open;
        }
    }

    private void Open()
    {
        door.Open();
        HUD.instance.SetInfo("");
        Interactions.instance.DoInteraction -= Open;
    }
}
