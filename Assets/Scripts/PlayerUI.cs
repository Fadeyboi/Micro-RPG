using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerUI : MonoBehaviour
{
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI inventoryText;
    public TextMeshProUGUI interactText;
    public Image healthbarFill;
    public Image xpbarFill;


    private Player player;

    void Awake()
    {
        player = FindAnyObjectByType<Player>();
    }

    public void UpdateLevelText()
    {
        levelText.text = "Level\n" + player.currentLevel;
    }

    public void UpdateHealthbar()
    {
        healthbarFill.fillAmount = (float)player.currentHP / (float)player.maxHP;
    }

    public void UpdateXpbar()
    {
        xpbarFill.fillAmount = (float)player.currentXp / (float)player.xpToNextLevel;
    }

    public void UpdateInventoryText()
    {
        inventoryText.text = "";
        foreach (string item in player.inventory)
        {
            inventoryText.text += item + "\n";
        }
    }

    public void SetInteractText(Vector3 pos, string text)
    {
        interactText.gameObject.SetActive(true);
        interactText.text = text;

        interactText.transform.position = Camera.main.WorldToScreenPoint(pos + Vector3.up);
    }

    public void DisableInteractText()
    {
        if (interactText.gameObject.activeInHierarchy)
        {
            interactText.gameObject.SetActive(false);
        }
    }
}
