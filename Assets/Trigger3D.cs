using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger3D : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) HUD.instance.Win();
    }
}
