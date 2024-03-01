using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public bool opening;
    public bool closing;
    public bool isOpen = false;

    public float openingSpeed = 5f;
    public float amountToMove = 4f;
    public Vector3 originalPos;
    [SerializeField] private AudioSource audioSource;


    private void Start()
    {
        originalPos = transform.position;
    }

    public void Open()
    {
        opening = true;
        closing = false;
        SoundManager.Instance.Play("DoorOpen", audioSource);
    }

    public void Close()
    {
        closing = true;
        opening = false;
    }

    private void Update()
    {
        if (isOpen && transform.position.y <= originalPos.y) Open();
        if (!isOpen && transform.position.y >= originalPos.y + amountToMove) Close();
        if (opening || closing)
        {
            int dir = opening ? 1 : -1;
            transform.Translate(0, dir *openingSpeed * Time.deltaTime, 0);

            if (opening && transform.position.y >= originalPos.y + amountToMove)
            {
                opening = false;
                isOpen = true;
            }
            else if (closing && transform.position.y <= originalPos.y)
            {
                closing = false;
                isOpen = false;
            }
        }
    }
}
