using UnityEngine;
using UnityEngine.UI;

public class Health_Manager : MonoBehaviour
{
    private Player_Data playerData;
    public Slider healthBar;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void SetHealthToPlayer(Player_Data data)
    {
        playerData = data;
        UpdateHealthUI();
    }

    // Update is called once per frame
    private void UpdateHealthUI()
    {
        if (healthBar != null)
            healthBar.value = playerData.currentHealth / playerData.maxHealth;
    }
}
