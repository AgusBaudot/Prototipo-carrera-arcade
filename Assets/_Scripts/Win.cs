using UnityEngine;
using UnityEngine.EventSystems;

public class Win : MonoBehaviour
{
    [SerializeField] private EventSystem _eventSystem;
    [SerializeField] private GameObject _defaultButton;
    [SerializeField] private Timer _timer;
    
    private void Start()
    {
        float previousScore = PlayerPrefs.HasKey("High score") ? PlayerPrefs.GetFloat("High score") : float.MaxValue;
        if (_timer.TotalTime < previousScore)
        {
            PlayerPrefs.SetFloat("High score", _timer.TotalTime); 
            PlayerPrefs.Save();
        }
            
        _eventSystem.SetSelectedGameObject(_defaultButton);
    }
}
