using System.Collections;
using Cinemachine;
using UnityEngine;

public class CameraShaker : MonoBehaviour
{
    [SerializeField, Range(0f, 15f)] private float _maxAmplitude;
    [SerializeField, Range(0, 0.5f)] private float _duration;

    private const float MinAmplitude = 0f;
    
    private Animator _animator;
    private Coroutine _shakeCoroutine;
    
    public void Shake()
    {
        if (_shakeCoroutine != null)
            StopCoroutine(_shakeCoroutine);

        _shakeCoroutine = StartCoroutine(ShakeCoroutine());
    }

    private IEnumerator ShakeCoroutine()
    {
        if (TryGetPerlin(out CinemachineBasicMultiChannelPerlin perlin))
        {
            perlin.m_AmplitudeGain = _maxAmplitude;

            yield return new WaitForSeconds(_duration);

            perlin.m_AmplitudeGain = MinAmplitude;
        }
    }

    private bool TryGetPerlin(out CinemachineBasicMultiChannelPerlin perlin)
    {
        perlin = CameraController.Instance.CurrentCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        
        return perlin != null;
    }
}