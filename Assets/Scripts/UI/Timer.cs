using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI timer_Text;
    public float remaining_Time; // Accessible by GameManager
    private bool isTimerRunning = false;

    public void ResetTimer(float seconds)
    {
        remaining_Time = seconds;
        isTimerRunning = true;
    }

    void Update()
    {
        if (!isTimerRunning) return;

        if (remaining_Time > 0)
        {
            remaining_Time -= Time.deltaTime;
        }
        else
        {
            remaining_Time = 0;
            isTimerRunning = false;
            GameManager.instance.OnTimeExpired();
        }

        DisplayTime(remaining_Time);
    }

    void DisplayTime(float timeToDisplay)
    {
        int minutes = Mathf.FloorToInt(timeToDisplay / 60);
        int seconds = Mathf.FloorToInt(timeToDisplay % 60);
        timer_Text.text = string.Format("{0:00}:{1:00}", minutes, seconds);

        // Visual warning if under 30 seconds
        timer_Text.color = (timeToDisplay < 30f) ? Color.red : Color.white;
    }
}