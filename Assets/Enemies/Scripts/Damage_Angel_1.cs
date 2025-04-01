using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    public float damageAmount = 10f;
    public float attackCooldown = 1f;

    private float nextAttackTime = 0f;

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && Time.time >= nextAttackTime)
        {
            Health_Manager playerHealth = FindAnyObjectByType<Health_Manager>();

            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damageAmount);
                nextAttackTime = Time.time + attackCooldown;
            }
            else
            {
                Debug.LogError("Health_Manager introuvable !");
            }
        }
    }
}
