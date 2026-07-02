using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 6.0f;
    public float rotationSpeed = 10.0f; // Velocidad con la que el personaje se voltea
    public float gravity = -9.81f;

    [Header("Stats Settings")]
    public float health = 50.0f; // Vida del personaje principal

    [Header("3rd Person Camera Settings")]
    public Transform playerCamera;
    public float cameraDistance = 5.0f; // Distancia de la cámara al jugador
    public float cameraHeight = 1.5f;   // Altura a la que mira la cámara
    public float mouseSensitivity = 2.0f;
    public float pitchMin = -20.0f;     // Límite inferior de la cámara (mirar hacia arriba)
    public float pitchMax = 60.0f;      // Límite superior de la cámara (mirar hacia abajo)

    [Header("Shooting Settings")]
    public GameObject bulletPrefab; // Aquí pones el Prefab de la bala
    public Transform firePoint;

    [Header("Ammo Settings")]
    public int maxAmmo = 20;
    private int currentAmmo;
    public Text ammoText;

    private CharacterController characterController;
    private float verticalVelocity;

    // Variables de rotación de la cámara
    private float yaw = 0f;
    private float pitch = 0f;
    private Camera camComponent;

    void Start()
    {
        characterController = GetComponent<CharacterController>();

        // 1. Intenta sacar la cámara del objeto asignado
        if (playerCamera != null)
        {
            camComponent = playerCamera.GetComponent<Camera>();

            // Si el objeto que arrastraste no era la cámara directa, busca en sus hijos
            if (camComponent == null)
            {
                camComponent = playerCamera.GetComponentInChildren<Camera>();
            }
        }

        // 2. Si todavía no hay cámara (porque te olvidaste de asignarla), busca la Main Camera automática de Unity
        if (camComponent == null)
        {
            camComponent = Camera.main;
        }

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (playerCamera != null && playerCamera.parent == transform)
        {
            playerCamera.parent = null;
        }

        currentAmmo = maxAmmo;
        UpdateAmmoHUD();
    }

    void Update()
    {
        // 1. Manejo del movimiento (WASD / Flechas) relativo a la cámara
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        // Obtener direcciones de la cámara ignorando la inclinación vertical (Y)
        Vector3 camForward = playerCamera.forward;
        Vector3 camRight = playerCamera.right;
        camForward.y = 0;
        camRight.y = 0;
        camForward.Normalize();
        camRight.Normalize();

        // Calcular dirección de movimiento final
        Vector3 moveDir = (camForward * vertical + camRight * horizontal).normalized;

        // 2. Aplicar Gravedad
        if (characterController.isGrounded)
        {
            verticalVelocity = -2f; // Fuerza constante hacia abajo para mantenerlo pegado al suelo
        }
        else
        {
            verticalVelocity += gravity * Time.deltaTime;
        }

        Vector3 velocity = moveDir * moveSpeed;
        velocity.y = verticalVelocity;

        // 3. Rotar al jugador para que mire hacia donde se está moviendo
        if (moveDir.sqrMagnitude > 0.01f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDir);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        // 4. Mover al personaje
        characterController.Move(velocity * Time.deltaTime);

        if (Input.GetButtonDown("Fire1") && currentAmmo > 0)
        {
            Shoot();
        }
    }

    void Shoot()
    {
        if (bulletPrefab == null || firePoint == null || camComponent == null) return;

        // Descontar bala y actualizar texto
        currentAmmo--;
        UpdateAmmoHUD();

        Ray ray = camComponent.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;
        Vector3 targetPoint;

        if (Physics.Raycast(ray, out hit))
        {
            targetPoint = hit.point;
        }
        else
        {
            targetPoint = ray.GetPoint(75);
        }

        Vector3 direction = targetPoint - firePoint.position;

        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        bullet.transform.forward = direction.normalized;
    }

    void UpdateAmmoHUD()
    {
        if (ammoText != null)
        {
            ammoText.text = "Balas " + currentAmmo + "/" + maxAmmo;
        }
    }

    void LateUpdate()
    {
        // 5. Órbita de la Cámara en 3era persona
        if (playerCamera != null)
        {
            yaw += Input.GetAxis("Mouse X") * mouseSensitivity;
            pitch -= Input.GetAxis("Mouse Y") * mouseSensitivity;
            pitch = Mathf.Clamp(pitch, pitchMin, pitchMax);

            // Definir el punto al que mirará la cámara (el centro/cabeza del jugador)
            Vector3 targetPosition = transform.position + Vector3.up * cameraHeight;

            // Calcular rotación y posición offset
            Quaternion camRotation = Quaternion.Euler(pitch, yaw, 0);
            Vector3 camDirection = new Vector3(0, 0, -cameraDistance);

            // Aplicar nueva posición y hacer que mire al jugador
            playerCamera.position = targetPosition + camRotation * camDirection;
            playerCamera.LookAt(targetPosition);
        }
    }
}