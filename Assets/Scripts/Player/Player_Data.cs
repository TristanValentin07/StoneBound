using UnityEngine;

public class Player_Data
{
    public float maxHealth = 100f;
    public float maxStamina = 100f;

    public float currentHealth;
    public float currentStamina;
    
    public float staminaDrainRate = 10f;
    public float staminaRecoveryRate = 8f;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Player_Data()
    {
        currentHealth = maxHealth;
        currentStamina = maxStamina;
    }
}
