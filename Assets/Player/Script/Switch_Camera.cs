using UnityEngine;

public class Switch_Camera : MonoBehaviour
{
    private Camera fpsCamera;
    private Camera tpsCamera;

    private GameObject fpsCamGO;
    private GameObject tpsCamGO;

    private AudioListener fpsAudio;
    private AudioListener tpsAudio;

    public bool isThirdPerson = false; // ← Mis en public pour accès depuis d'autres scripts

    public delegate void ViewChanged(bool isThirdPersonView);
    public static event ViewChanged OnViewChanged; 
    public static Switch_Camera Instance { get; private set; }


    void Start()
    {
        Instance = this;

        fpsCamGO = transform.Find("CameraHolder/Player_Camera")?.gameObject;
        tpsCamGO = GameObject.Find("CameraThirdHolder/Third_person_cam");

        if (fpsCamGO == null)
            Debug.LogError("❌ FPS Camera GameObject introuvable");
        if (tpsCamGO == null)
            Debug.LogError("❌ TPS Camera GameObject introuvable");

        fpsCamera = fpsCamGO?.GetComponent<Camera>();
        tpsCamera = tpsCamGO?.GetComponent<Camera>();

        if (fpsCamera == null)
            Debug.LogError("❌ Composant Camera manquant sur FPS");
        if (tpsCamera == null)
            Debug.LogError("❌ Composant Camera manquant sur TPS");

        fpsAudio = fpsCamGO?.GetComponent<AudioListener>();
        tpsAudio = tpsCamGO?.GetComponent<AudioListener>();

        SetCameraState();
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            isThirdPerson = !isThirdPerson;
            SetCameraState();

            // 🔔 Avertit les autres scripts du changement de vue
            OnViewChanged?.Invoke(isThirdPerson);
        }
    }

    void SetCameraState()
    {
        bool isFPS = !isThirdPerson;

        if (fpsCamGO != null) fpsCamGO.SetActive(isFPS);
        if (tpsCamGO != null) tpsCamGO.SetActive(!isFPS);

        if (fpsCamera != null) fpsCamera.enabled = isFPS;
        if (tpsCamera != null) tpsCamera.enabled = !isFPS;

        if (fpsAudio != null) fpsAudio.enabled = isFPS;
        if (tpsAudio != null) tpsAudio.enabled = !isFPS;
    }
}
