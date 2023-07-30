using System.Collections;
using UnityEngine;

public class RippleEffect : MonoBehaviour
{
    [Header("Ripple material")]
    [SerializeField] private Material _material;
    
    [Header("Variables")]
    [SerializeField, Range(0f, 1.5f)] private float _time;
    
    private const float MaxDistance = 1f;
    private const float MinDistance = -0.1f;

    private readonly int _rippleDistance = Shader.PropertyToID("_RippleDistance");

    public void Show()
    {
        StartCoroutine(ChangeDistance());
    }

    private IEnumerator ChangeDistance()
    {
        _material.SetFloat(_rippleDistance, MinDistance);

        float lerpedAmount = 0f;
        float elapsedTime = 0f;

        while (elapsedTime < _time)
        {
            elapsedTime += Time.deltaTime;

            lerpedAmount = Mathf.Lerp(MinDistance, MaxDistance, (elapsedTime / _time));
            _material.SetFloat(_rippleDistance, lerpedAmount);

            yield return null;
        }
    }
}
