using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    public int puntosDeCura = 5; // Cuánta vida recupera el botiquín

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            
            Salud saludJugador = other.GetComponent<Salud>();

            if (saludJugador != null)
            {
                saludJugador.Curar(puntosDeCura);
                Destroy(gameObject);
            }
        }
    }
}