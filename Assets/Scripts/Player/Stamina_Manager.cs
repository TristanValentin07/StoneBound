using UnityEngine;
using UnityEngine.UI;

public class Stamina_Manager : MonoBehaviour
{
    private Player_Data playerData;
    public Slider staminaBar;
    
    public bool infiniteStamina = false;
    
    public void SetStaminaToPlayer(Player_Data data)
    {
        playerData = data;
        UpdateStaminaBar();
    }
    
    public void UpdateStaminaBar()
    {
        if (staminaBar != null && playerData != null)
        {
            if (infiniteStamina == true)
            {
                playerData.currentStamina = playerData.maxStamina;
            }
            staminaBar.value = playerData.currentStamina / playerData.maxStamina;
        }
        else
        {
            Debug.LogError(" Problème : `staminaBar` ou `playerData` est NULL !");
        }
    }
}
