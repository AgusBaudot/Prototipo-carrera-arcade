using System;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    private TextMeshProUGUI _timerText;
    private float _timer;
    private TimeSpan _timeSpan;
    private bool _runTimer = true;

    public float TotalTime => _timer;

    private void Awake()
    {
        _timerText = GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        if (_runTimer)
        {
            _timer += Time.deltaTime;
            _timeSpan = TimeSpan.FromSeconds(_timer);
            if (_timeSpan.Seconds < 10)
                _timerText.text = $"0{_timeSpan.Minutes}:0{_timeSpan.Seconds}.{_timeSpan.Milliseconds}";
            else
                _timerText.text = $"0{_timeSpan.Minutes}:{_timeSpan.Seconds}.{_timeSpan.Milliseconds}";
        }
    }

    public void StopTimer() => _runTimer = false;
}