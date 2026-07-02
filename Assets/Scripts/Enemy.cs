using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : MonoBehaviour
{
    [Header("Enemy Stats")]
    public float health = 10.0f;

    [Header("Attack Settings")]
    public int damageToPlayer = 10;     // Tu script recibe un entero (int)
    public float attackRange = 2.0f;
    public float attackCooldown = 1.0f;
    private float nextAttackTime = 0f;

    private Transform playerTarget;
    private NavMeshAgent navAgent;
    private Salud playerSalud;

    [Header("Drop Settings")]
    public GameObject ammoDropPrefab;
    public float ammodropChance = 0.3f;
    public GameObject healthDropPrefab;
    public float healthDropChance = 0.40f;

    void Start()
    {
        navAgent = GetComponent<NavMeshAgent>();
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");

        if (playerObj != null)
        {
            playerTarget = playerObj.transform;
            playerSalud = playerObj.GetComponent<Salud>(); // Obtiene tu script de vida
        }
    }

    void Update()
    {
        if (playerTarget != null && playerSalud != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, playerTarget.position);

            if (distanceToPlayer > attackRange)
            {
                navAgent.SetDestination(playerTarget.position);
            }
            else
            {
                if (Time.time >= nextAttackTime)
                {
                    playerSalud.Danar(damageToPlayer); // Llama a tu función
                    nextAttackTime = Time.time + attackCooldown;
                }
            }
        }
    }

    // Se mantiene esto para que la bala pueda matar al enemigo
    public void TakeDamage(float damageAmount)
    {
        health -= damageAmount;
        if (health <= 0)
        {
            float randomRoll = Random.value;

            // Si cae en el primer 40%, suelta botiquín
            if (healthDropPrefab != null && randomRoll <= healthDropChance)
            {
                Instantiate(healthDropPrefab, transform.position + (Vector3.up * 0.5f), Quaternion.identity);
            }
            // Si no fue botiquín, verificamos si cae en el siguiente 30% para las balas
            else if (ammoDropPrefab != null && randomRoll <= (healthDropChance + ammodropChance))
            {
                Instantiate(ammoDropPrefab, transform.position + (Vector3.up * 0.5f), Quaternion.identity);
            }

            // --- AVISO AL ADMINISTRADOR DE RONDAS ---
            if (OleadasManager.inst != null)
            {
                OleadasManager.inst.EnemigoMuerto();
            }

            Destroy(gameObject);
        }
    }
}