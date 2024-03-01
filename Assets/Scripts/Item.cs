using UnityEngine;

[CreateAssetMenu(fileName = "General Item", menuName = "Items/New General Item")]
public class Item : ScriptableObject
{
    public new string name;
    public PickUp dropObject;
    public ItemObject useObject;
}