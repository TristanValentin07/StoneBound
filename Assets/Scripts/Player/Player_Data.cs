using UnityEngine;

public class Player_Data
{
    public float maxHealth = 100f;
    public float maxStamina = 100f;

    public float currentHealth;
    public float currentStamina;
    
    public float staminaDrainRate = 10f;
    public float staminaRecoveryRate = 15f;
    
    public Player_Data()
    {
        currentHealth = maxHealth;
        currentStamina = maxStamina;
    }
}
