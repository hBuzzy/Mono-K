using System.Collections;
using UnityEngine;

public class DeathScreen : MonoBehaviour
{
    [SerializeField] private Material _deathScreenMaterial;
    [SerializeField] private float _changeTime;
 
    private const float MinRadius = 0f;
    private const float MaxRadius = 40f;
    
    private readonly int _maskRadius = Shader.PropertyToID("_MaskRadius");
    
    private Coroutine _changeRadiusCoroutine;

    private void Start()
    {
        _deathScreenMaterial.SetFloat(_maskRadius, MinRadius);
    }

    public IEnumerator Show()
    {
        yield return ChangeVisibility(MaxRadius);
    }

    public IEnumerator Hide()
    {
        yield return ChangeVisibility(MinRadius);
    }

    private IEnumerator ChangeVisibility(float maskRadius)
    {
        if (_changeRadiusCoroutine != null)
            StopCoroutine(_changeRadiusCoroutine);

        _changeRadiusCoroutine = StartCoroutine(ChangeRadius(maskRadius));
        
        yield return _changeRadiusCoroutine;
    }

    private IEnumerator ChangeRadius(float targetRadius)
    {
        float elapsedTime = 0f;
        float startRadius = _deathScreenMaterial.GetFloat(_maskRadius);

        while (elapsedTime < _changeTime)
        {
            float currentRadius = Mathf.Lerp(startRadius, targetRadius, (elapsedTime / _changeTime));
            _deathScreenMaterial.SetFloat(_maskRadius, currentRadius);

            elapsedTime += Time.deltaTime;
            
            yield return null;
        }
        
        _deathScreenMaterial.SetFloat(_maskRadius, targetRadius);
    }
}