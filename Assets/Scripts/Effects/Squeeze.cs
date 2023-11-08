using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

public class Squeeze : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _characterSprite;
    [SerializeField] private Vector2 _stretchSize;
    
    [Header("Times")]
    [SerializeField, Range(0f, 1f)] private float _stretchDuration;
    [SerializeField, Range(0f, 1f)] private float _compressDuration;
    
    private Coroutine _squeezeCoroutine;

    public void Play()
    {
        if (_squeezeCoroutine != null)
            StopCoroutine(_squeezeCoroutine);

        _squeezeCoroutine = StartCoroutine(SqueezeJump(_stretchSize.x, _stretchSize.y));
    }

    private IEnumerator SqueezeJump(float xSqueeze, float ySqueeze, float dropAmount = 0)
    {
        Vector3 originalSize = transform.localScale;
        Vector3 newSize = new Vector3(xSqueeze, ySqueeze, originalSize.z);
        
        Vector3 originalPosition = Vector3.zero;
        Vector3 newPosition = new Vector3(0, -dropAmount, 0);

        yield return ChangeValue(originalSize, newSize, originalPosition, newPosition, _stretchDuration);
        yield return ChangeValue(newSize, originalSize, newPosition, originalPosition, _compressDuration);
    }

    private IEnumerator ChangeValue(Vector3 sizeFrom, Vector3 sizeTo, Vector3 positionFrom, Vector3 positionTo, float seconds)
    {
        float time = 0f;
        float maxTime = 1f;

        while (time <= maxTime)
        {
            _characterSprite.transform.localScale = Vector3.Lerp(sizeFrom, sizeTo, time);
            _characterSprite.transform.localPosition = Vector3.Lerp(positionFrom, positionTo, time);

            time += Time.deltaTime / seconds;
            
            yield return null;
        }
    }
}