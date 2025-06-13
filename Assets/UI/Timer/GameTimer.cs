using UnityEngine;
using UnityEngine.UI;

public class GameTimer : MonoBehaviour
{
    public float duration = 60f;
    public Text timerText;

    private float timeRemaining;
    private bool running = true;

    void Start()
    {
        if (timerText == null)
            Debug.LogError("[GameTimer] timerText non assigné !");
        timeRemaining = duration;
        UpdateDisplay();
    }

    void Update()
    {
        if (!running) return;

        timeRemaining -= Time.deltaTime;
        if (timeRemaining <= 0f)
        {
            timeRemaining = 0f;
            running = false;
            UpdateDisplay();
            OnTimerEnd();
        }
        else
        {
            UpdateDisplay();
        }
    }

    void UpdateDisplay()
    {
        int minutes = Mathf.FloorToInt(timeRemaining / 60f);
        int seconds = Mathf.FloorToInt(timeRemaining % 60f);
        timerText.text = $"{minutes:00}:{seconds:00}";
    }

    void OnTimerEnd()
    {
        Debug.Log("[GameTimer] Le temps est écoulé !");
        
        var hm = FindFirstObjectByType<Health_Manager>();
        if (hm != null)
        {
            hm.TakeDamage(1000);
        }
    }
}