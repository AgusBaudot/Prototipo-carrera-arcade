using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonSelectionUX : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    [SerializeField] private bool up;
    [SerializeField] private int offset;
    [SerializeField] private int speed;
    
    private Vector3 _originalPosition;
    private bool _selected;
    private TextMeshProUGUI _textUGUI;

    private void Awake()
    {
        _originalPosition = transform.localPosition;
        _textUGUI = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
    }

    public void OnSelect(BaseEventData eventData) => _selected = true;

    public void OnDeselect(BaseEventData eventData) => _selected = false;

    private void Update()
    {
        _textUGUI.color = _selected ? Color.white : Color.black;
        Vector3 targetPos = _originalPosition;
        if (up && _selected)
        {
            targetPos = _originalPosition + Vector3.up * offset;
        }
        if (!up && _selected)
        {
            targetPos = _originalPosition + Vector3.right * offset;
        }
        
        transform.localPosition = Vector3.MoveTowards(transform.localPosition, targetPos, speed * Time.deltaTime);
    }
}
