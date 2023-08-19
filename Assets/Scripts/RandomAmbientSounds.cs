using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class RandomAmbientSounds : MonoBehaviour
{
    [Header("Random delay between sounds")]
    [SerializeField] private float _minDelay;
    [SerializeField] private float _maxDelay;

    [SerializeField] private AudioSource[] _ambientSounds;

    private Coroutine _waitCoroutine;
    private float _delay;

    private void Start()
    {
        _waitCoroutine = StartCoroutine(Wait());
    }

    private void Update()
    {
        if (_waitCoroutine != null)
            return;
        
        AudioSource ambientSound = GetRandomSound();
        ambientSound.PlayOneShot(ambientSound.clip);

        _waitCoroutine = StartCoroutine(Wait());
    }

    private IEnumerator Wait()
    {
        yield return new WaitForSeconds(GetDelay());

        _waitCoroutine = null;
    }

    private float GetDelay()
    {
        return Random.Range(_minDelay, _maxDelay);
    }

    private AudioSource GetRandomSound()
    {
        return _ambientSounds[Random.Range(0, _ambientSounds.Length)];
    }
}
