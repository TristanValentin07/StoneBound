using UnityEngine;
using UnityEngine.AI;

public class Boss_AI : MonoBehaviour
{
    private Transform player;
    private NavMeshAgent agent;

    [Header("Spawn Prism")]
    public GameObject bossPrism;

    [Header("Stats")]
    public float detectionRange = 20f;
    public float stopDistance = 2f;
    public int maxHealth = 100;

    private int currentHealth;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        currentHealth = maxHealth;
        InvokeRepeating(nameof(FindPlayer), 0f, 1f);
    }

    void FindPlayer()
    {
        var found = GameObject.FindWithTag("Player");
        if (found != null) player = found.transform;
    }

    void Update()
    {
        if (player == null) return;

        float dist = Vector3.Distance(transform.position, player.position);
        if (dist < detectionRange && dist > stopDistance)
        {
            if (NavMesh.SamplePosition(player.position, out var hit, 1f, NavMesh.AllAreas))
                agent.SetDestination(hit.position);
            else
                agent.ResetPath();
        }
        else agent.ResetPath();
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        Debug.Log($"{name} a pris {amount} dégâts. Vie restante : {currentHealth}");
        if (currentHealth <= 0) Die();
    }

    void Die()
    {
        bossPrism.SetActive(true);
        Destroy(gameObject);
    }
}
