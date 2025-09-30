using UnityEngine;

public class SpeedBoost : MonoBehaviour
{
    [Header("Speed")]
    [SerializeField] private float baseSpeed = 2f;
    [SerializeField] private float extraPerTap = 0.5f;
    [SerializeField] private float decayRate = 1f;
    [SerializeField] private float maxExtra = 5f;
    
    private float _extra = 0f;
    
    public float BaseSpeed => baseSpeed;
    public float ExtraSpeed => _extra;
    public float MaxExtra => maxExtra;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _extra = Mathf.Min(_extra + extraPerTap, maxExtra);
        }

        _extra = Mathf.MoveTowards(_extra, 0f, decayRate * Time.deltaTime);
    }

    public float GetMoveSpeed() => baseSpeed + _extra;
}
