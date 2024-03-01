using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] float damage = 5f;
    
    private void OnParticleCollision(GameObject other)
    {
        Debug.Log("Hit someone: " + other.tag);
        switch (other.tag)
        {
            case "Player":
                other.GetComponent<Player>()?.TakeDamage(damage);
                break;
            // TODO: Hit effects...
        }
    }
}
