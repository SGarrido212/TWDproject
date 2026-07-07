using UnityEngine;
using UnityEngine.UI;

public class OleadasManager : MonoBehaviour
{
    public static OleadasManager inst; 

    [Header("Configuración de Rondas")]
    public int rondasMaximas = 3;
    private int rondaActual = 1;

    [Header("Spawners y Enemigos")]
    public GameObject enemyPrefab;
    public Transform[] spawnPoints;      
    public int enemigosPorRondaBase = 5; // Enemigos en la primera ronda (luego se multiplica)
    private int enemigosVivos = 0;

    [Header("UI")]
    public Text textoRonda; // El texto del HUD

    void Awake()
    {
       
        inst = this;
    }

    void Start()
    {
        EmpezarRonda();
    }

    void EmpezarRonda()
    {
        ActualizarUI();

        
        int cantidadEnemigos = enemigosPorRondaBase * rondaActual;
        enemigosVivos = cantidadEnemigos;

        for (int i = 0; i < cantidadEnemigos; i++)
        {
            // Elegir un spawn point al azar de entre los 4 disponibles
            Transform puntoElegido = spawnPoints[Random.Range(0, spawnPoints.Length)];

            // Un pequeño margen aleatorio para que no aparezcan pegados uno dentro del otro
            Vector2 randomCircle = Random.insideUnitCircle * 2f;
            Vector3 spawnPosition = new Vector3(
                puntoElegido.position.x + randomCircle.x,
                puntoElegido.position.y,
                puntoElegido.position.z + randomCircle.y
            );

            Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
        }
    }

    
    public void EnemigoMuerto()
    {
        enemigosVivos--;

        
        if (enemigosVivos <= 0)
        {
            SiguienteRonda();
        }
    }

    void SiguienteRonda()
    {
        rondaActual++;

        if (rondaActual > rondasMaximas)
        {
            if (textoRonda != null) textoRonda.text = "¡Victoria!";
            
        }
        else
        {
            EmpezarRonda();
        }
    }

    void ActualizarUI()
    {
        if (textoRonda != null)
        {
            textoRonda.text = "Ronda " + rondaActual + "/" + rondasMaximas;
        }
    }
}