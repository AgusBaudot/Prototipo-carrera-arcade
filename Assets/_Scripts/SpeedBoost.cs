using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class SpeedBoost : MonoBehaviour
{
    [Header("Speed")]
    [SerializeField] private float baseSpeed = 2f;
    [SerializeField] private float extraPerTap = 0.5f;
    [SerializeField] private float decayRate = 1f;
    [SerializeField] private float maxExtra = 5f;
    [SerializeField] private float stumbleAmount = 0.5f;
    [Header("Speed horizontal")]
    [SerializeField] private TextMeshProUGUI speedText;

    public bool IsLocked => _locked;
    private bool _locked;
    
    private float _extra;
    private int _lastPercentage = -1;
    
    public float BaseSpeed => baseSpeed;
    public float ExtraSpeed => _extra;
    public float MaxExtra => maxExtra;

    private void Update()
    {
        if (!_locked && Input.GetKeyDown(KeyCode.Space))
        {
            _extra = Mathf.Min(_extra + extraPerTap, maxExtra);
        }

        _extra = Mathf.MoveTowards(_extra, 0f, decayRate * Time.deltaTime);
        
        float percentage = Mathf.InverseLerp(0, maxExtra, _extra);
        percentage = Mathf.Lerp(0, 100, percentage);
        int rounded = (int)Math.Round(percentage / 10.0) * 10;

        if (rounded != _lastPercentage)
        {
            speedText.text = $"Speed: {rounded:0}%";
            _lastPercentage = rounded;
        }
    }

    public float GetMoveSpeed() => baseSpeed + _extra;
    
    private void ResetExtra() => _extra = 0f;

    public void SetLocked(bool locked)
    {
        _locked = locked;
        ResetExtra();
        StartCoroutine(Stumble());
    }

    private IEnumerator Stumble()
    {
        yield return new WaitForSeconds(stumbleAmount);
        _locked = false;
    }
}
