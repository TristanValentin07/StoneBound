using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using TMPro;

public class MenuPageManager : MonoBehaviour
{
    [Header("Menu Pages")]
    public GameObject mainPage;
    public GameObject difficultyPage;
    public GameObject optionsPage;
    
    [Header("Scene")]
    [SerializeField] private string SceneToLoad;

    [Header("UI Elements")]
    public TMP_Dropdown difficultyDropdown;

    void Start()
    {
        ShowMainPage();

        if (difficultyDropdown != null)
        {
            var options = new List<string> { "Easy", "Medium", "Hard" };
            difficultyDropdown.ClearOptions();
            difficultyDropdown.AddOptions(options);

            difficultyDropdown.value = (int)GameSettings.difficulty;
            difficultyDropdown.RefreshShownValue();

            difficultyDropdown.onValueChanged.AddListener(OnDropdownChanged);
        }
    }

    public void ShowDifficultyPage()
    {
        Debug.Log("Showing difficulty Page");
        mainPage.SetActive(false);
        difficultyPage.SetActive(true);
        optionsPage.SetActive(false);
    }

    public void ShowOptionsPage()
    {
        mainPage.SetActive(false);
        difficultyPage.SetActive(false);
        optionsPage.SetActive(true);
    }

    public void ShowMainPage()
    {
        mainPage.SetActive(true);
        difficultyPage.SetActive(false);
        optionsPage.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
    #endif
    }
    
    public void SelectEasy()
    {
        Debug.Log("Difficulté : Easy");
        GameSettings.difficulty = Difficulty.Easy;
        StartGame();
    }
    
    public void SelectMedium()
    {
        Debug.Log("Difficulté : Medium");
        GameSettings.difficulty = Difficulty.Medium;
        StartGame();
    }
    
    public void SelectHard()
    {
        Debug.Log("Difficulté : Hard");
        GameSettings.difficulty = Difficulty.Hard;
        StartGame();
    }

    public void StartGame()
    {
        Debug.Log("Chargement de la scène : " + SceneToLoad);
        SceneManager.LoadScene(SceneToLoad);
    }

    private void OnDropdownChanged(int index)
    {
        GameSettings.difficulty = (Difficulty)index;
        Debug.Log($"[Menu] Difficulty set via dropdown → {GameSettings.difficulty}");
    }
}
