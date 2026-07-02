using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    public int puntosDeCura = 5; // Cuánta vida recupera el botiquín

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Busca tu script de Salud en el jugador
            Salud saludJugador = other.GetComponent<Salud>();

            if (saludJugador != null)
            {
                saludJugador.Curar(puntosDeCura);
                Destroy(gameObject); // El botiquín desaparece al recogerse
            }
        }
    }
}