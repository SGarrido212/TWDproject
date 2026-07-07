using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 20f; 
    public float lifeTime = 3f; 
    public float damage = 10f;

    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    void OnTriggerEnter(Collider other)
    {
        // Si choca contra algo que tenga el tag "Enemy"
        if (other.CompareTag("Enemy"))
        {
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage); 
            }

            Destroy(gameObject); 
        }
    }
}