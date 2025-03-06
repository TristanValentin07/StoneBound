using UnityEngine;
using UnityEngine.SceneManagement;

public class MapManager : MonoBehaviour
{
    public string mapToLoad;

    void Start()
    {
        LoadMap();
    }

    public void LoadMap()
    {
        if (!string.IsNullOrEmpty(mapToLoad))
        {
            SceneManager.LoadScene(mapToLoad, LoadSceneMode.Single);
        }
        else
        {
            Debug.LogError("Aucune map spécifiée !");
        }
    }
}
