using UnityEngine;

public class Switch_Camera : MonoBehaviour
{
    private Camera fpsCamera;
    private Camera tpsCamera;

    private GameObject fpsCamGO;
    private GameObject tpsCamGO;

    private AudioListener fpsAudio;
    private AudioListener tpsAudio;

    private bool isThirdPerson = false;

    void Start()
    {
        fpsCamGO = transform.Find("Player_Camera")?.gameObject;

        GameObject tpsGO = GameObject.Find("Third_person_cam");
        tpsCamGO = tpsGO;

        if (fpsCamGO == null || tpsCamGO == null)
        {
            Debug.LogError("Impossible de trouver les caméras !");
            return;
        }

        fpsCamera = fpsCamGO.GetComponent<Camera>();
        tpsCamera = tpsCamGO.GetComponent<Camera>();

        fpsAudio = fpsCamGO.GetComponent<AudioListener>();
        tpsAudio = tpsCamGO.GetComponent<AudioListener>();

        SetCameraState();
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            isThirdPerson = !isThirdPerson;
            SetCameraState();
        }
    }

    void SetCameraState()
    {
        fpsCamGO.SetActive(!isThirdPerson);
        tpsCamGO.SetActive(isThirdPerson);

        if (fpsCamera) fpsCamera.enabled = !isThirdPerson;
        if (tpsCamera) tpsCamera.enabled = isThirdPerson;

        if (fpsAudio) fpsAudio.enabled = !isThirdPerson;
        if (tpsAudio) tpsAudio.enabled = isThirdPerson;
    }
}