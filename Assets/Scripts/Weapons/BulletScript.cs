using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int damage = 50;

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("💥 Balle a touché : " + collision.collider.name);

        EnemyAI enemy = collision.collider.GetComponent<EnemyAI>();
        Boss_AI boss = collision.collider.GetComponent<Boss_AI>();
        if (enemy != null)
        {
            Debug.Log("✅ Ennemi détecté, on inflige des dégâts !");
            enemy.TakeDamage(damage);
        } else if (boss != null)
        {
            Debug.Log("Shoot BOSS !");
            boss.TakeDamage(damage);
        }
        Destroy(gameObject);
    }
}
