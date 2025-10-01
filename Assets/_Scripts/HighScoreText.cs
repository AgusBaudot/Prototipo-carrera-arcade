using System;
using TMPro;
using UnityEngine;

public class HighScoreText : MonoBehaviour
{
    private void Start()
    {
        float _timer = PlayerPrefs.GetFloat("High score", 0);
        var timeText = TimeSpan.FromSeconds(_timer);
        
        TextMeshProUGUI _timerText = GetComponent<TextMeshProUGUI>(); 
        
        if (timeText.Seconds < 10)
            _timerText.text = $"Personal best: 0{timeText.Minutes}:0{timeText.Seconds}.{timeText.Milliseconds}";
        else
            _timerText.text = $"Personal best: 0{timeText.Minutes}:{timeText.Seconds}.{timeText.Milliseconds}";
    }
}
