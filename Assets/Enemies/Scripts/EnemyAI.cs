using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
public class EnemyAI : MonoBehaviour
{
    private Transform player;
    private NavMeshAgent agent;
    public float detectionRange = 20f;
    public float stopDistance = 2f;
    public int maxHealth = 100;
    
    private int currentHealth;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        currentHealth = maxHealth;

        InvokeRepeating("FindPlayer", 0f, 1f);
    }


    void FindPlayer()
    {
        GameObject foundPlayer = GameObject.FindGameObjectWithTag("Player");
        if (foundPlayer != null)
        {
            player = foundPlayer.transform;
        }
    }

    void Update()
    {
        if (player == null)
        {
            return;
        }

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer < detectionRange && distanceToPlayer > stopDistance)
        {
            if (NavMesh.SamplePosition(player.position, out NavMeshHit hit, 1.0f, NavMesh.AllAreas))
            {
                agent.SetDestination(hit.position);
            }
            else
            {
                agent.ResetPath();
            }
        }
        else
        {
            agent.ResetPath();
        }
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;

        if (currentHealth <= 0)
            Die();

        Debug.Log($"{gameObject.name} a pris {amount} de dégâts. Vie restante : {currentHealth}");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log($"{gameObject.name} est mort !");

        EnemySpawner spawner = FindAnyObjectByType<EnemySpawner>();
        if (spawner != null)
        {
            spawner.ReturnEnemy(this.gameObject);
        }
        else
        {
            Debug.LogWarning("❌ Aucun EnemySpawner trouvé, destruction forcée !");
            Destroy(gameObject);
        }
    }
}
