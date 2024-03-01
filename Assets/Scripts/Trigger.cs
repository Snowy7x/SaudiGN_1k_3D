using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger : MonoBehaviour
{
    private bool isDone = false;
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player") && !isDone)
        {
            StartCoroutine(Done());
            isDone = true;
        }
    }

    IEnumerator Done()
    {
        MiniGame.instance.Pre_Done();
        yield return new WaitForSeconds(0.5f);
        MiniGame.instance.Done();
    }
}
