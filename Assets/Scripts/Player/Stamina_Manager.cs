using UnityEngine;
using UnityEngine.UI;

public class Stamina_Manager : MonoBehaviour
{
    private Player_Data playerData;
    public Slider staminaBar;
    
    public void SetStaminaToPlayer(Player_Data data)
    {
        playerData = data;
        UpdateStaminaBar();
    }
    
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
