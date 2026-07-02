using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Configuración del Spawner")]
    public GameObject enemyPrefab;    // Aquí arrastrarás el clon del enemigo (Prefab)
    public int numberOfEnemies = 5;   // Cantidad de enemigos a generar
    public float spawnRadius = 2.0f;  // Qué tan juntos/separados van a aparecer

    void Start()
    {
        // Llama a la función al iniciar el juego
        SpawnEnemyGroup();
    }

    public void SpawnEnemyGroup()
    {
        if (enemyPrefab == null)
        {
            Debug.LogError("Por favor, asigna el Prefab del enemigo en el Inspector.");
            return;
        }

        for (int i = 0; i < numberOfEnemies; i++)
        {
            // Genera una posición aleatoria en un círculo plano (ejes X y Z)
            Vector2 randomCircle = Random.insideUnitCircle * spawnRadius;

            // Creamos la posición final sumando el offset a la posición del Spawner
            Vector3 spawnPosition = new Vector3(
                transform.position.x + randomCircle.x,
                transform.position.y,                  // Mantiene la misma altura del Spawner
                transform.position.z + randomCircle.y
            );

            // Instancia (crea) el enemigo en la escena
            Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
        }
    }
}