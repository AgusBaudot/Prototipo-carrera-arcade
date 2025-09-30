using UnityEngine;

// [CreateAssetMenu(menuName = "Movement/Wind Disturbance Profile")]
// public class WindProfile : ScriptableObject
// {
//     public float maxAmplitude = 0.4f;
//     public float frequency = 0.8f; //Low frequency will mean gentler wind.
//     public AnimationCurve strengthCurve = new AnimationCurve(new Keyframe(0f, 0f), new Keyframe(1f, 1f));
// }

[RequireComponent(typeof(Transform))]
public class WindDisturbance : MonoBehaviour
{
    [Header("Thresholds")]
    [SerializeField, Range(0f, 0.99f)]
    private float ignoreBelow = 0.25f;   // no wind below this normalized speed

    [Header("Push (constant)")]
    [Tooltip("How strong the wind pushes at full normalized speed (units/sec).")]
    [SerializeField] private float strength = 1.0f; // <-- now interpreted as velocity (units/sec)

    [Header("Smoothness")]
    [Tooltip("Seconds it takes to smoothly reach the target push (bigger = slower, gentler)")]
    [SerializeField] private float smoothTime = 0.6f;

    [Header("Direction flip timing (seconds)")]
    [SerializeField] private float minFlipTime = 0.8f;
    [SerializeField] private float maxFlipTime = 1.6f;

    private float _currentVelocity = 0f; // current lateral velocity (units/sec), positive = right
    private float _velocityRef = 0f;     // for SmoothDamp
    private float _direction = 1f;       // chosen direction: +1 or -1
    private float _flipTimer = 0f;
    private float _flipDuration = 1f;
    private System.Random _rng;

    private void Awake()
    {
        _rng = new System.Random(System.Environment.TickCount ^ gameObject.GetInstanceID());
        PlanNextFlip(initial: true);
    }

    private void FixedUpdate()
    {
        _flipTimer += Time.fixedDeltaTime;
        if (_flipTimer >= _flipDuration)
            PlanNextFlip(initial: false);

        // Nothing else here — SampleWindVelocity will be used by MovementController each FixedUpdate
    }

    private void PlanNextFlip(bool initial)
    {
        // keep the direction for the gust duration (randomized)
        _direction = _rng.Next(0, 2) == 0 ? 1f : -1f;
        float t = (float)_rng.NextDouble();
        _flipDuration = Mathf.Lerp(minFlipTime, maxFlipTime, t);
        _flipTimer = 0f;

        if (initial)
            _currentVelocity = 0f;
    }

    /// <summary>
    /// NEW API: Call every FixedUpdate with normalizedSpeed (0..1).
    /// Returns lateral velocity in world units/second (positive = right).
    /// MovementController should integrate this (windVel * Time.fixedDeltaTime) into the player's lateral position.
    /// </summary>
    public float SampleWindVelocity(float normalizedSpeed)
    {
        Debug.Log(normalizedSpeed);
        normalizedSpeed = Mathf.Clamp01(normalizedSpeed);

        float targetVel = 0f;
        if (normalizedSpeed > ignoreBelow)
        {
            float scaled = (normalizedSpeed - ignoreBelow) / (1f - ignoreBelow); // 0..1
            targetVel = _direction * strength * scaled; // velocity in units/sec
        }

        // Smoothly approach the target velocity (so push ramps up/down smoothly)
        _currentVelocity = Mathf.SmoothDamp(_currentVelocity, targetVel, ref _velocityRef, Mathf.Max(0.0001f, smoothTime), Mathf.Infinity, Time.fixedDeltaTime);
        return _currentVelocity;
    }
}
