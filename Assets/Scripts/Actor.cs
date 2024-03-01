using UnityEngine;

public class Actor : MonoBehaviour, IDamageable
{
    public float health = 100f;
    public float maxHealth = 100f;
    public bool isAlive = true;
    
    public virtual void TakeDamage(float damage)
    {
        health = Mathf.Max(0f, health - damage);
        if (health == 0) Die();
    }

    public virtual void AddHealth(float amount)
    {
        health += amount;
    }

    public virtual void Die()
    {
        isAlive = false;
    }
}