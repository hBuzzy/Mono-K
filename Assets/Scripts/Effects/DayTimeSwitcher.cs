using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class DayTimeSwitcher : MonoBehaviour
{
    [SerializeField] private NightTrigger _nightTrigger;
    [SerializeField] private DayTrigger _dayTrigger;
    
    [Header("Light options")]
    [SerializeField] private Light2D _globalLight;
    [SerializeField, Range(0.1f, 1.5f)] private float _speedChange;
    [SerializeField, Range(0.7f, 1f)] private float _dayIntensity; 
    [SerializeField, Range(0.03f, 0.08f)] private float _nightIntensity;
    
    private Coroutine _coroutine;

    private void Start()
    {
        _globalLight.intensity = _dayIntensity;
    }

    private void OnEnable()
    {
        _nightTrigger.Triggered += OnNightTriggered;
        _dayTrigger.Triggered += OnDayTriggered;
    }

    private void OnDisable()
    {
        _nightTrigger.Triggered -= OnNightTriggered;
        _dayTrigger.Triggered -= OnDayTriggered;
    }

    private void ChangeLightIntensity(float intensity)
    {
        if (_coroutine != null)
            StopCoroutine(_coroutine);

        _coroutine = StartCoroutine(ChangeValue(intensity));
    }

    private IEnumerator ChangeValue(float target)
    {
        while (_globalLight.intensity != target)
        {
            _globalLight.intensity = Mathf.MoveTowards(_globalLight.intensity, target, _speedChange * Time.deltaTime);
            yield return null;
        }
    }

    private void OnNightTriggered()
    {
        ChangeLightIntensity(_nightIntensity);
    }

    private void OnDayTriggered()
    {
        ChangeLightIntensity(_dayIntensity);
    }
}
