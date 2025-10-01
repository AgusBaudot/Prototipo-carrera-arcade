using System;
using UnityEngine;
using UnityEngine.Events;

public class LaneGuard : MonoBehaviour
{
    [SerializeField] private float laneHalfWidth = 1f;
    [SerializeField] private UnityEvent onDisqualified;

    private MovementController _movementController;
    private SpeedBoost _speedBoost;
    private WindDisturbance _wind;
    private bool _disqualified;

    private void Awake()
    {
        _movementController =  GetComponent<MovementController>();
        _speedBoost = GetComponent<SpeedBoost>();
        _wind = GetComponent<WindDisturbance>();
    }

    private void FixedUpdate()
    {
        if (_disqualified) return;
        
        if (Mathf.Abs(transform.position.x) > laneHalfWidth)
        {
            _disqualified = true;
            onDisqualified?.Invoke();
            Destroy(gameObject);
        }
    }
    
    public void ResetGuard() => _disqualified = false;
}
