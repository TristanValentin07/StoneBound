using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int damage = 100;

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("💥 Balle a touché : " + collision.collider.name);

        EnemyAI enemy = collision.collider.GetComponent<EnemyAI>();
        if (enemy != null)
        {
            Debug.Log("✅ Ennemi détecté, on inflige des dégâts !");
            enemy.TakeDamage(damage);
        }

        Destroy(gameObject);
    }
}
