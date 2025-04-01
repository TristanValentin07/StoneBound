using UnityEngine;

public class ThirdPersonFollowLook : MonoBehaviour
{
    public Transform playerBody; // Le joueur
    public float mouseSensitivity = 100f;

    public Vector3 offset = new Vector3(0, 2, -4); // Position de la caméra
    public float smoothTime = 0.05f;

    private float _xRotation = 0f;
    private Vector3 _velocity = Vector3.zero;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        if (Cursor.lockState != CursorLockMode.Locked) return;

        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // Rotation verticale
        _xRotation -= mouseY;
        _xRotation = Mathf.Clamp(_xRotation, -40f, 60f);

        // Tourner le joueur horizontalement
        playerBody.Rotate(Vector3.up * mouseX);
    }

    void LateUpdate()
    {
        if (playerBody == null) return;

        // Calcul de la position cible de la caméra
        Quaternion rotation = Quaternion.Euler(_xRotation, playerBody.eulerAngles.y, 0);
        Vector3 desiredPosition = playerBody.position + rotation * offset;

        // Suivi fluide
        transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref _velocity, smoothTime);

        // Toujours regarder le joueur
        transform.LookAt(playerBody.position + Vector3.up * 1.5f);
    }
}