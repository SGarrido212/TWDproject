using UnityEngine;
using UnityEngine.AI; // Necesario para usar NavMeshAgent

[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : MonoBehaviour
{
    [Header("Stats Settings")]
    public float health = 10.0f; // Vida de cada enemigo
    private Transform playerTarget;
    private NavMeshAgent navAgent;

    void Start()
    {
        // 1. Obtener la referencia del NavMeshAgent
        navAgent = GetComponent<NavMeshAgent>();

        // 2. Buscar automáticamente al jugador en la escena usando el Tag
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            playerTarget = player.transform;
        }
        else
        {
            Debug.LogError("No se encontró ningún objeto con el tag 'Player' en la escena.");
        }
    }

    void Update()
    {
        // 3. Actualizar constantemente el destino del agente hacia la posición del jugador
        if (playerTarget != null)
        {
            navAgent.SetDestination(playerTarget.position);
        }
    }
    public void TakeDamage(float damageAmount)
    {
        health -= damageAmount;

        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
}