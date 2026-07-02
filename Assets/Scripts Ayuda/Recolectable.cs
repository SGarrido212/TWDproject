using UnityEngine;

public class Recolectable : MonoBehaviour
{
    public int valor;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            other.GetComponent<Puntaje>().SumarPuntaje(valor);
        }
    }
}
