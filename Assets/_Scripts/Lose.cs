using UnityEngine;
using UnityEngine.EventSystems;

public class Lose : MonoBehaviour
{
    [SerializeField] private EventSystem _eventSystem;
    [SerializeField] private GameObject _defaultButton;
    
    private void Start()
    {
        _eventSystem.SetSelectedGameObject(_defaultButton);
    }
}