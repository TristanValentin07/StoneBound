using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class FakeInstagramPublisher : MonoBehaviour
{
    [Header("UI Elements")]
    public Button openPanelButton; // bouton "Partager sur Instagram" dans le win screen
    public GameObject instagramPanel;
    public TMP_InputField instagramInputField;
    public Button confirmShareButton;
    public Button returnButton;

    [Header("Capture Settings")]
    public string screenshotFileName = "score_capture.jpg";

    private Texture2D capturedImage;
    private string username;

#if UNITY_STANDALONE_WIN
    private string GetDownloadPath()
    {
        return Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.UserProfile), "Downloads");
    }
#else
    private string GetDownloadPath()
    {
        return Application.persistentDataPath;
    }
#endif

    void Start()
    {
        if (openPanelButton != null)
            openPanelButton.onClick.AddListener(OnOpenPanelClicked);

        if (confirmShareButton != null)
            confirmShareButton.onClick.AddListener(OnConfirmShareClicked);

        if (returnButton != null)
            returnButton.onClick.AddListener(() => instagramPanel.SetActive(false));

        instagramPanel.SetActive(false);
    }

    void OnOpenPanelClicked()
    {
        StartCoroutine(CaptureScreenshotBeforePanel());
    }

    IEnumerator CaptureScreenshotBeforePanel()
    {
        yield return new WaitForEndOfFrame();

        int width = Screen.width;
        int height = Screen.height;
        capturedImage = new Texture2D(width, height, TextureFormat.RGB24, false);
        capturedImage.ReadPixels(new Rect(0, 0, width, height), 0, 0);
        capturedImage.Apply();

        Debug.Log("📸 Capture prise avant ouverture du panel.");
        instagramPanel.SetActive(true);
        StartCoroutine(ForceCursorVisibleDelayed());
    }

    void OnConfirmShareClicked()
    {
        username = instagramInputField.text;

        if (string.IsNullOrWhiteSpace(username))
        {
            Debug.LogWarning("⚠️ Veuillez entrer un pseudo Instagram.");
            return;
        }

        string path = Path.Combine(GetDownloadPath(), screenshotFileName);
        byte[] imageBytes = capturedImage.EncodeToJPG();
        File.WriteAllBytes(path, imageBytes);

        Debug.Log("✅ Capture enregistrée dans : " + path);
        Debug.Log("📤 Simulation de post sur Instagram pour @" + username);

           //POST https://graph.facebook.com/v19.0/{ig-user-id}/media
           //Params : image_url, caption, access_token
           
           //POST https://graph.facebook.com/v19.0/{ig-user-id}/media_publish
           //Params : creation_id, access_token
           
        instagramPanel.SetActive(false);
    }
    
    IEnumerator ForceCursorVisibleDelayed()
    {
        yield return new WaitForSeconds(0.1f);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

}
