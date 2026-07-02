using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 6.0f;
    public float rotationSpeed = 10.0f;
    public float gravity = -9.81f;

    [Header("3rd Person Camera Settings")]
    public Transform playerCamera;
    public float cameraDistance = 5.0f;
    public float cameraHeight = 1.5f;
    public float mouseSensitivity = 2.0f;
    public float pitchMin = -20.0f;
    public float pitchMax = 60.0f;

    [Header("Shooting Settings")]
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float fireRate = 0.5f;
    private float nextTimeToFire = 0f;

    [Header("Ammo Settings")]
    public int maxAmmo = 20;
    private int currentAmmo;
    public Text ammoText;

    private CharacterController characterController;
    private float verticalVelocity;
    private float yaw = 0f;
    private float pitch = 0f;
    private Camera camComponent;

    void Start()
    {
        characterController = GetComponent<CharacterController>();

        if (playerCamera != null)
        {
            camComponent = playerCamera.GetComponent<Camera>();
            if (camComponent == null) camComponent = playerCamera.GetComponentInChildren<Camera>();
        }
        if (camComponent == null) camComponent = Camera.main;

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
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 camForward = camComponent.transform.forward;
        Vector3 camRight = camComponent.transform.right;
        camForward.y = 0;
        camRight.y = 0;
        camForward.Normalize();
        camRight.Normalize();

        Vector3 moveDir = (camForward * vertical + camRight * horizontal).normalized;

        if (characterController.isGrounded) verticalVelocity = -2f;
        else verticalVelocity += gravity * Time.deltaTime;

        Vector3 velocity = moveDir * moveSpeed;
        velocity.y = verticalVelocity;

        if (moveDir.sqrMagnitude > 0.01f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDir);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        characterController.Move(velocity * Time.deltaTime);

        if (Input.GetButtonDown("Fire1") && currentAmmo > 0 && Time.time >= nextTimeToFire)
        {
            nextTimeToFire = Time.time + fireRate;
            Shoot();
        }
    }

    void Shoot()
    {
        if (bulletPrefab == null || firePoint == null || camComponent == null) return;

        currentAmmo--;
        UpdateAmmoHUD();

        Ray ray = camComponent.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;
        Vector3 targetPoint = Physics.Raycast(ray, out hit) ? hit.point : ray.GetPoint(75);

        Vector3 direction = targetPoint - firePoint.position;
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        bullet.transform.forward = direction.normalized;
    }

    void UpdateAmmoHUD()
    {
        if (ammoText != null) ammoText.text = "Balas " + currentAmmo + "/" + maxAmmo;
    }

    void LateUpdate()
    {
        if (playerCamera == null) return;

        yaw += Input.GetAxis("Mouse X") * mouseSensitivity;
        pitch -= Input.GetAxis("Mouse Y") * mouseSensitivity;
        pitch = Mathf.Clamp(pitch, pitchMin, pitchMax);

        Vector3 targetPosition = transform.position + Vector3.up * cameraHeight;
        Quaternion camRotation = Quaternion.Euler(pitch, yaw, 0);
        Vector3 camDirection = new Vector3(0, 0, -cameraDistance);

        playerCamera.position = targetPosition + camRotation * camDirection;
        playerCamera.LookAt(targetPosition);
    }
}