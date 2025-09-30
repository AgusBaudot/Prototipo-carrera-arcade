using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class JumpController : MonoBehaviour
{
    [SerializeField] private float baseJumpForce = 5f;
    [SerializeField] private float globalMultiplier = 1f;
    [SerializeField] private float groundCheckDistance = 0.15f;
    [SerializeField] private AnimationCurve jumpCurve = new AnimationCurve(new Keyframe(0f, 1f), new Keyframe(1f, 1.5f));
    [SerializeField] private LayerMask groundLayer;

    private Rigidbody _rb;
    private SpeedBoost _speedBoost;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _speedBoost = GetComponent<SpeedBoost>();
    }
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow) && IsGrounded())
        {
            float normalized = Mathf.InverseLerp(_speedBoost.BaseSpeed, _speedBoost.BaseSpeed + _speedBoost.MaxExtra,
                _speedBoost.GetMoveSpeed());
            _rb.AddForce(Vector3.up * (baseJumpForce * jumpCurve.Evaluate(normalized) * globalMultiplier), ForceMode.Impulse);
        }
    }

    public bool IsGrounded() => Physics.Raycast(transform.position, Vector3.down, groundCheckDistance, groundLayer);
}
