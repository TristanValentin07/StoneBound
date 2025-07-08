using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public GameObject pausePanel; // Référence au panel dans le Canvas
    public Button resumeButton;
    public Button backToMenuButton;
    public string menuSceneName = "Menu"; // Nom de la scène du menu

    private bool isPaused = false;

    void Start()
    {
        pausePanel.SetActive(false);

        // Attribue les fonctions aux boutons
        resumeButton.onClick.AddListener(ResumeGame);
        backToMenuButton.onClick.AddListener(BackToMenu);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
                ResumeGame();
            else
                PauseGame();
        }
    }

    public void PauseGame()
    {
        pausePanel.SetActive(true);
        Time.timeScale = 0f; // temps passé à 0
        isPaused = true;

        // Débloque le curseur
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void ResumeGame()
    {
        pausePanel.SetActive(false);
        Time.timeScale = 1f; // Reprend le jeu
        isPaused = false;

        // Verrouille à nouveau le curseur
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void BackToMenu()
    {
        Time.timeScale = 1f; // Important avant de changer de scène
        SceneManager.LoadScene(menuSceneName);
    }
}
