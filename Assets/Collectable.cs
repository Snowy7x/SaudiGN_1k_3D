using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    [SerializeField] GameObject ps;
    private void Update()
    {
        transform.Rotate (Vector3.up * 50 * Time.deltaTime, Space.Self);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<Actor>()?.AddHealth(50f);
            Instantiate(ps, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            Destroy(gameObject);
            MiniGame.instance.AddCoin();
        }
    }
}
