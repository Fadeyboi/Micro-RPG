using UnityEngine;

public class Item : MonoBehaviour
{
    public string itemName;

    public void PickupItem()
    {
        FindAnyObjectByType<Player>().AddItemToInventory(itemName);
        gameObject.SetActive(false);
    }
}
