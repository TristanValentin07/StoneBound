using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    private Transform player;
    private NavMeshAgent agent;
    public float detectionRange = 20f;
    public float stopDistance = 2f;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        if (agent == null)
        {
            Debug.LogError("[EnemyAI] ❌ NavMeshAgent manquant sur : " + gameObject.name);
        }

        InvokeRepeating("FindPlayer", 0f, 1f); // Cherche le joueur chaque seconde
    }

    void FindPlayer()
    {
        GameObject foundPlayer = GameObject.FindGameObjectWithTag("Player");
        if (foundPlayer != null)
        {
            player = foundPlayer.transform;
            Debug.Log("[EnemyAI] ✅ Joueur trouvé : " + player.name);
        }
        else
        {
            Debug.LogWarning("[EnemyAI] ⚠️ Joueur non trouvé !");
        }
    }

    void Update()
    {
        if (player == null)
        {
            Debug.LogWarning("[EnemyAI] ⚠️ Pas de joueur assigné.");
            return;
        }

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        Debug.Log("[EnemyAI] 📏 Distance au joueur : " + distanceToPlayer);

        if (distanceToPlayer < detectionRange && distanceToPlayer > stopDistance)
        {
            if (NavMesh.SamplePosition(player.position, out NavMeshHit hit, 1.0f, NavMesh.AllAreas))
            {
                agent.SetDestination(hit.position);
                Debug.Log("[EnemyAI] 🚶 Se dirige vers le joueur !");
            }
            else
            {
                Debug.LogWarning("[EnemyAI] 🚫 Impossible d'aller vers le joueur (hors NavMesh).");
                agent.ResetPath();
            }
        }
        else
        {
            Debug.Log("[EnemyAI] ⏹️ Arrêt du mouvement (trop proche ou trop loin).");
            agent.ResetPath();
        }
    }
}