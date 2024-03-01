using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorDoor : MonoBehaviour
{
    public Elevator left;
    public Animator anim;
    public ElvPlace place;
    public bool just_got_out = false;
    [SerializeField] private AudioSource audioSource;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && just_got_out) return;
        if (other.CompareTag("Player") && left.GetPlace() == place)
        {
            HUD.instance.SetInfo("press [e] to open the elevator");
            Interactions.instance.DoInteraction += Open_Inter;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && just_got_out)
        {
            just_got_out = false;
            Close();
        }
        if (other.CompareTag("Player"))
        {
            if (left.GetPlace() == place)
            {
                HUD.instance.SetInfo("");
                Interactions.instance.DoInteraction -= Open_Inter;
            }
        }
    }

    private void Open_Inter()
    {
        HUD.instance.SetInfo("");
        Interactions.instance.DoInteraction -= Open_Inter;
        Open();
    }

    public void Open()
    {
        anim.Play("Open");
        SoundManager.Instance.Play("LeftOpen", audioSource);
    }

    public void Close()
    {
        anim.Play("Close");
        SoundManager.Instance.Play("LeftClose", audioSource);
    }

    public bool IsClosed()
    {
        return anim.GetCurrentAnimatorStateInfo(0).IsName("Closed");
    }
}
