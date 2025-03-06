using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Health_Manager : MonoBehaviour
{
    private Player_Data playerData;
    public Slider healthBar;
    public Image DeadScreen;
    private string sceneToLoad = "Menu";
    
    private bool isDead = false;

    public void SetHealthToPlayer(Player_Data data)
    {
        playerData = data;
        UpdateHealthUI();
    }
    private void UpdateHealthUI()
    {
        if (healthBar != null)
            healthBar.value = playerData.currentHealth / playerData.maxHealth;
    }
    
    public void TakeDamage(float amount)
    {
        if (playerData == null || isDead) return;

        playerData.currentHealth -= amount;
        Debug.Log($"[Health_Manager] 💥 Vie après dégâts : {playerData.currentHealth}/{playerData.maxHealth}");

        UpdateHealthUI(); // Mets à jour l'UI ici

        if (playerData.currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        if (isDead) return;
        isDead = true;

        Debug.Log("[Health_Manager] ❌ GAME OVER !");
        
        if (DeadScreen != null)
        {
            DeadScreen.gameObject.SetActive(true);
            Debug.Log("[Health_Manager] 🟥 Écran de mort activé !");
        }
        else
        {
            Debug.LogError("[Health_Manager] ❌ gameOverScreen non assigné !");
        }

        Time.timeScale = 0;

        StartCoroutine(ReturnToMenu());
    }

    
    private IEnumerator ReturnToMenu()
    {
        yield return new WaitForSecondsRealtime(3f);
        //code hyper technique
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 1;
        SceneManager.LoadScene(sceneToLoad);
    }
}
