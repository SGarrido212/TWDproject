using UnityEngine;

public class AmmoPickup : MonoBehaviour
{
    public int balasARecargar = 10;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController player = other.GetComponent<PlayerController>();

            if (player != null)
            {
                player.AddAmmo(balasARecargar);
                Destroy(gameObject);
            }
        }
    }
}