using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class CaveSounds : MonoBehaviour
{
    [SerializeField] private AudioSource _waterDrop;
    [SerializeField] private float _minDelay;
    [SerializeField] private float _maxDelay;
    
    private float _delay;

    private void Start()
    {
        _delay = GetDelay();
    }

    private void Update()
    {
        _delay -= Time.deltaTime;

        if (_delay > 0)
        {
            return;
        }

        _waterDrop.PlayOneShot(_waterDrop.clip);

        _delay = GetDelay();
    }

    private float GetDelay()
    {
        return Random.Range(_minDelay, _maxDelay);
    }
}
