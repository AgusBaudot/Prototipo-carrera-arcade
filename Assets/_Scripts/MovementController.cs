using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class MovementController : MonoBehaviour
{
    [Header("Lateral Control")]
    [SerializeField] private float lateralSpeed = 3f;
    [SerializeField] private float lateralClamp = 1f;

    private Rigidbody _rb;
    private SpeedBoost _speedBoost;
    private LateralInput _lateralInput;
    private WindDisturbance _wind;
    private JumpController _jump;

    private float _playerLateral = 0f;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _speedBoost = GetComponent<SpeedBoost>();
        _lateralInput = GetComponent<LateralInput>();
        _wind = GetComponent<WindDisturbance>();
        _jump = GetComponent<JumpController>();
    }

    private void FixedUpdate()
    {
        Vector3 forward = Vector3.forward * (_speedBoost.GetMoveSpeed() * Time.fixedDeltaTime);

        // Only read new input when grounded
        if (_jump.IsGrounded())
        {
            _playerLateral += _lateralInput.ReadSmoothed() * lateralSpeed * Time.fixedDeltaTime;
            _playerLateral = Mathf.Clamp(_playerLateral, -lateralClamp, lateralClamp);

            float wind = _wind.SampleWindVelocity(Mathf.InverseLerp(
                _speedBoost.BaseSpeed,
                _speedBoost.BaseSpeed + _speedBoost.MaxExtra,
                _speedBoost.GetMoveSpeed()
            ));

            // Apply wind only on ground
            _rb.MovePosition(new Vector3(_playerLateral + wind * Time.fixedDeltaTime, _rb.position.y, _rb.position.z) + forward);
        }
        else
        {
            // When in air → keep current X, don’t snap back
            _rb.MovePosition(new Vector3(_rb.position.x, _rb.position.y, _rb.position.z) + forward);
        }
        // Vector3 forward = Vector3.forward * (_speedBoost.GetMoveSpeed() * Time.fixedDeltaTime);
        //
        // _playerLateral += _lateralInput.ReadSmoothed() * lateralSpeed * Time.fixedDeltaTime;
        // _playerLateral = Mathf.Clamp(_playerLateral, -lateralClamp, lateralClamp);
        //
        // Vector3 lateral = Vector3.zero;
        // if (_jump.IsGrounded())
        // {
        //     float wind = _wind.SampleWindVelocity(Mathf.InverseLerp(_speedBoost.BaseSpeed,
        //         _speedBoost.BaseSpeed + _speedBoost.MaxExtra, _speedBoost.GetMoveSpeed()));
        //     lateral = transform.right * (_playerLateral + wind * Time.fixedDeltaTime);
        // }
        //
        // _rb.MovePosition(_rb.position + forward + lateral);
    }
    
    public float GetIntentionalLateral() => Math.Abs(transform.position.x);
}
