using UnityEngine;

public class CameraLook : MonoBehaviour
{
    public Transform playerBody; // Référence au joueur
    public Transform cameraHolder; // Pivot de la caméra qui suit le joueur
    public float mouseSensitivity = 100f;
    public float cameraHeightOffset = 1.8f; // Hauteur de la caméra
    public float avancementCamera = 0.2f;

    private float _xRotation = 0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        if (Cursor.lockState != CursorLockMode.Locked) return;

        // Récupère le mouvement de la souris
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
    
        // Rotation verticale (haut/bas), appliquée uniquement sur la caméra
        _xRotation -= mouseY;
        _xRotation = Mathf.Clamp(_xRotation, -90f, 90f);
        transform.localRotation = Quaternion.Euler(_xRotation, 0f, 0f);

        // Rotation horizontale (gauche/droite) du joueur, forcée par la caméra
        Quaternion newRotation = Quaternion.Euler(0f, playerBody.rotation.eulerAngles.y + mouseX, 0f);
        playerBody.rotation = newRotation;
    }

    void LateUpdate()
    {
        // Déplace la caméra à la bonne position, mais NE TOUCHE PAS À SA ROTATION
        Vector3 basePosition = playerBody.position + Vector3.up * cameraHeightOffset;
        Vector3 forwardOffset = playerBody.forward * avancementCamera;
        // Appliquer la position avancée à la caméra
        transform.position = basePosition + forwardOffset;
    }
}
