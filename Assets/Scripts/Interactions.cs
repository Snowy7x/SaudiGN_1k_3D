using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Interactions : MonoBehaviour
{
    public static Interactions instance;
    public delegate void Interaction();
    public event Interaction DoInteraction;

    private void Awake()
    {
        instance = this;
    }

    public void Interact()
    {
        if (DoInteraction != null) DoInteraction();
    }
}
