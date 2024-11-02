using UnityEngine;

public class Chest : MonoBehaviour
{
    private int xpToGive;

    public void OpenChest()
    {
        xpToGive = (int)(FindAnyObjectByType<Enemy>().xpToGive * 1.5);
        FindAnyObjectByType<Player>().AddXp(xpToGive);
        gameObject.SetActive(false);
    }
}
