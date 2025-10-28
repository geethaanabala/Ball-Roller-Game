using System;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("Timer display")]
    public TextMeshProUGUI timerTextTMP; // assign in inspector
    public GameObject levelCompletePanel; // assign a panel that is disabled by default
    public TextMeshProUGUI finishTimeTextTMP;

    private float elapsed = 0f;
    private bool running = false;

    void Update()
    {
        if (!running) return;
        elapsed += Time.deltaTime;
        UpdateTimerDisplay(elapsed);
    }

    public void StartTimer()
    {
        elapsed = 0f;
        running = true;
        if (levelCompletePanel != null) levelCompletePanel.SetActive(false);
        UpdateTimerDisplay(0f);
    }

    // Overload: stop and optionally provide finalTime
    public void StopTimer(float finalTime = -1f)
    {
        running = false;
        if (finalTime >= 0f) elapsed = finalTime;
        UpdateTimerDisplay(elapsed);
    }

    void UpdateTimerDisplay(float t)
    {
        // Format as mm:ss.ff
        TimeSpan ts = TimeSpan.FromSeconds(t);
        string s = string.Format("{0:D2}:{1:D2}.{2:D2}", ts.Minutes, ts.Seconds, ts.Milliseconds / 10);
        if (timerTextTMP != null) timerTextTMP.text = s;
    }

    public void ShowLevelComplete(float time)
    {
        if (levelCompletePanel != null) levelCompletePanel.SetActive(true);
        if (finishTimeTextTMP != null)
        {
            TimeSpan ts = TimeSpan.FromSeconds(time);
            string s = string.Format("{0:D2}:{1:D2}.{2:D2}", ts.Minutes, ts.Seconds, ts.Milliseconds / 10);
            finishTimeTextTMP.text = "Time: " + s;
        }
    }
}
