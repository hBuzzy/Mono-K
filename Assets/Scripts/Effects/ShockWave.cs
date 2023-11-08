using System.Collections;
using UnityEngine;

public class ShockWave : MonoBehaviour
{
    [Header("Ripple material")]
    [SerializeField] private Material _material;

    [Header("Variables")]
    [SerializeField, Range(0f, 1.5f)] private float _time;

    private readonly int _rippleDistance = Shader.PropertyToID("_RippleDistance");

    private const float MaxDistance = 1f;
    private const float MinDistance = -0.1f;

    public void Show()
    {
        StartCoroutine(ChangeDistance());
    }

    private IEnumerator ChangeDistance()
    {
        _material.SetFloat(_rippleDistance, MinDistance);
        float elapsedTime = 0f;

        while (elapsedTime < _time)
        {
            float distance = Mathf.Lerp(MinDistance, MaxDistance, (elapsedTime / _time));
            _material.SetFloat(_rippleDistance, distance);

            elapsedTime += Time.deltaTime;
            
            yield return null;
        }
        
        _material.SetFloat(_rippleDistance, MaxDistance);
    }
}
