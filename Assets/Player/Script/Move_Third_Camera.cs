using UnityEngine;

public class ThirdPersonFollowLook : MonoBehaviour
{
    public Transform playerBody;
    public float mouseSensitivity = 100f;

    public Vector3 offset = new Vector3(0, 3f, -5f);
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
        _xRotation = Mathf.Clamp(_xRotation, -50f, 50f);

        // Tourner le joueur horizontalement
        playerBody.Rotate(Vector3.up * mouseX);
    }

    void LateUpdate()
    {
        if (playerBody == null) return;

        // Rotation de la caméra
        Quaternion rotation = Quaternion.Euler(_xRotation, playerBody.eulerAngles.y, 0);

        // On garde la hauteur fixe (offset.y), mais on applique la rotation seulement au plan XZ
        Vector3 flatOffset = new Vector3(offset.x, 0, offset.z); // ignore y
        Vector3 rotatedOffset = rotation * flatOffset;

        // On ajoute manuellement la hauteur
        Vector3 desiredPosition = playerBody.position + rotatedOffset + Vector3.up * offset.y;

        transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref _velocity, smoothTime);

        // La caméra regarde la tête du joueur
        transform.LookAt(playerBody.position + Vector3.up * 1.7f);
    }

}