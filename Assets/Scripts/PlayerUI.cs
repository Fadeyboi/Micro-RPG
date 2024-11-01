using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerUI : MonoBehaviour
{
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI inventoryText;
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
}
