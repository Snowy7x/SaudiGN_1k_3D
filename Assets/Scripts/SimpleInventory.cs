using Unity.VisualScripting;
using UnityEngine;

public class SimpleInventory : MonoBehaviour
{
    public Transform holder;
    public ItemObject currentItem;

    public void PickUp(Item item)
    {
        if (currentItem != null) Drop();
        ItemObject obj = Instantiate(item.useObject, holder);
        currentItem = obj;
    }

    public void Drop()
    {
        if (currentItem == null) return;
        PickUp pickUp = Instantiate(currentItem.item.dropObject, transform.position, transform.rotation);
        Destroy(currentItem);
        pickUp.GetComponent<Rigidbody>()?.AddExplosionForce(10f, transform.position, 5f);
    }

    private void Update()
    {
        // ....
        if (InputManager.Instance.fire && currentItem != null)
        {
            currentItem.Use();
        }
    }
}
