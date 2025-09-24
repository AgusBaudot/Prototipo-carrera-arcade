using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Speed Settings")]
    [SerializeField] private float baseSpeed = 2f;          // Always applied
    [SerializeField] private float extraSpeedPerTap = 0.5f; // How much each tap adds
    [SerializeField] private float decayRate = 1f;          // How fast speed decays per second
    [SerializeField] private float maxExtraSpeed = 5f;      // Cap for bonus speed
    
    [Header("Jump Settings")]
    [SerializeField] private float baseJumpForce = 5f;       // Baseline jump impulse
    [SerializeField] private float jumpForceMultiplier = 2f; // Additional global multiplier
    [SerializeField] private AnimationCurve jumpCurve = new AnimationCurve(
        new Keyframe(0f, 1f), new Keyframe(1f, 2f)
    ); // Maps normalized speed (0..1) to jump multiplier
    
    [Header("Ground Check")]
    [SerializeField] private LayerMask groundLayer;          // To detect ground
    [SerializeField] private float groundCheckDistance = 0.1f;

    [Header("Vibration / Instability")]
    [SerializeField] private bool vibrationEnabled = true;
    [SerializeField] private float maxLateralAmplitude = 0.5f; // max sideways offset in world units when at max speed
    [SerializeField] private float vibrationFrequency = 3f;    // how quickly the vibration moves
    [SerializeField] private AnimationCurve vibrationStrengthCurve = new AnimationCurve(
        new Keyframe(0f, 0f), new Keyframe(1f, 1f)
    ); // maps normalized speed to multiplier for amplitude (0 at baseSpeed)
    [SerializeField] private float lateralSmoothTime = 0.05f;  // smoothing time for lateral movement
    
    private float _currentExtraSpeed = 0f;
    private Rigidbody _rb;
    
    // Vibration internals
    private float _perlinSeed;
    private float _currentLateral = 0f;
    private float _lateralVel = 0f;

    private void Awake()
    {
        _rb =  GetComponent<Rigidbody>();
        // pick a random seed for Perlin so multiple players/instances jitter differently.
        _perlinSeed = Random.Range(0f, 1000f);
    }

    private void Update()
    {
        // Space tap boosts running speed
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _currentExtraSpeed += extraSpeedPerTap;
            _currentExtraSpeed = Mathf.Min(_currentExtraSpeed, maxExtraSpeed);
        }

        // Gradually decay extra speed
        _currentExtraSpeed = Mathf.MoveTowards(_currentExtraSpeed, 0f, decayRate * Time.deltaTime);
        
        // Jumping (only if on ground)
        if (Input.GetKeyDown(KeyCode.UpArrow) && IsGrounded())
        {
            // moveSpeed is the actual speed used for horizontal movement
            float moveSpeed = baseSpeed + _currentExtraSpeed;

            float normalized = maxExtraSpeed > 0
                ? Mathf.InverseLerp(baseSpeed, baseSpeed + maxExtraSpeed, moveSpeed)
                : 0f;
            
            // Arcade-y scaling via AnimationCurve:
            // curve value acts as multiplier for baseJumpForce
            float curveMultiplier = jumpCurve.Evaluate(normalized);
            
            //Use impulse so the jump feels snappy
            _rb.AddForce(Vector3.up * (baseJumpForce * curveMultiplier * jumpForceMultiplier),  ForceMode.Impulse);
        }
    }

    private void FixedUpdate()
    {
        float moveSpeed = baseSpeed + _currentExtraSpeed;
        
        Vector3 forwardMove = Vector3.forward * (moveSpeed * Time.fixedDeltaTime);

        // lateral vibration only if enabled and only if we have extra speed above base
        Vector3 lateralOffset = Vector3.zero;
        if (vibrationEnabled && IsGrounded())
        {
            // normalized speed: 0 when at baseSpeed, 1 when at base + maxExtraSpeed
            float normalized = maxExtraSpeed > 0f
                ? Mathf.InverseLerp(baseSpeed, baseSpeed + maxExtraSpeed, moveSpeed)
                : 0f;

            // Evaluate curve so you can shape how quickly instability rises with speed
            float strength = vibrationStrengthCurve.Evaluate(normalized);

            // Perlin noise for smooth pseudo-random oscillation in range [-1, 1]
            float t = Time.time * vibrationFrequency;
            float perlin = Mathf.PerlinNoise(t + _perlinSeed, _perlinSeed) * 2f - 1f;

            float targetLateral = perlin * maxLateralAmplitude * strength;

            // Smooth towards the target lateral offset for less jittery teleporting
            _currentLateral = Mathf.SmoothDamp(_currentLateral, targetLateral, ref _lateralVel, lateralSmoothTime);

            // Convert to world-space lateral offset (using local right so it respects rotation)
            lateralOffset = transform.right * _currentLateral;
        }

        // Compose new position, preserving Y from physics (we move only XZ)
        Vector3 targetPos = _rb.position + forwardMove + lateralOffset;

        _rb.MovePosition(targetPos);
    }

    private bool IsGrounded()
    {
        //Simple ground check
        return Physics.Raycast(transform.position, Vector3.down, groundCheckDistance, groundLayer);
    }
}