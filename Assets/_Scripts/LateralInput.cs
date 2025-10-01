using UnityEngine;

public class LateralInput : MonoBehaviour
{
    [Header("Input")]
    [SerializeField] private float inputSmoothTime = 0.05f;

    private SpeedBoost _speedBoost;
    private float _smoothed = 0f;
    private float _vel = 0f;

    public float ReadSmoothed()
    {
        _speedBoost??= GetComponent<SpeedBoost>();
        if (_speedBoost.IsLocked) return 0;
        
        _smoothed = Mathf.SmoothDamp(_smoothed, Input.GetAxisRaw("Horizontal"), ref _vel, inputSmoothTime);
        return Mathf.Clamp(_smoothed, -1f, 1f);
    }
}
