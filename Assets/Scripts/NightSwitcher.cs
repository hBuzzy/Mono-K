using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class NightSwitcher : MonoBehaviour
{
    [SerializeField] private Light2D _globalLight;
    [SerializeField] private float _speedChange;

    private bool isNight = false;

    public event Action NightStarted;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Character character))
        {
            if (isNight == false)
            {
                StartCoroutine(ChangeToNight(0.3f));
            }
        }
    }

    private IEnumerator ChangeToNight(float target)
    {
        while (_globalLight.intensity != target)
        {
            _globalLight.intensity = Mathf.MoveTowards(_globalLight.intensity, target, _speedChange * Time.deltaTime);
            yield return null;
        }
        
        NightStarted?.Invoke();
        isNight = true;
    }
}
