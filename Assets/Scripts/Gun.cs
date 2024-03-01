using UnityEngine;

[CreateAssetMenu(fileName = "New Gun", menuName = "Items/New Gun")]
public class Gun : Item
{
    public int ammo;
    public int maxAmmo;
    public float fireRate;
    public float fireRange;

    public bool isContinuous;
    public float damage = 30f;
}