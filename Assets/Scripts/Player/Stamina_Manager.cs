using UnityEngine;
using UnityEngine.UI;

public class Stamina_Manager : MonoBehaviour
{
    private Player_Data playerData;
    public Slider staminaBar;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void SetStaminaToPlayer(Player_Data data)
    {
        playerData = data;
        UpdateStaminaBar();
    }

    // Update is called once per frame
    public void UpdateStaminaBar()
    {
        if (staminaBar != null && playerData != null)
        {
            staminaBar.value = playerData.currentStamina / playerData.maxStamina;
        }
        else
        {
            Debug.LogError(" Problème : `staminaBar` ou `playerData` est NULL !");
        }
    }
}
