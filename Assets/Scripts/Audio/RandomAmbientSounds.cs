using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class RandomAmbientSounds : MonoBehaviour
{
    [Header("Random delay between sounds (in sec)")]
    [SerializeField] private float _minDelay;
    [SerializeField] private float _maxDelay;
    
    [Header("Components")]
    [SerializeField] private SoundZone _soundZone;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip[] _ambientSounds;
    
    private Coroutine _waitCoroutine;
    private float _delay;
    private bool _isCharacterInsideZone;

    private void Start()
    {
        _waitCoroutine = StartCoroutine(Wait());
    }
    
    private void OnEnable()
    {
        _soundZone.CharacterInsideChanged += OnCharacterInsideChanged;
    }

    private void OnDisable()
    {
        _soundZone.CharacterInsideChanged -= OnCharacterInsideChanged;
    }

    private void Update()
    {
        if (_isCharacterInsideZone == false || _waitCoroutine != null)
            return;

        _audioSource.PlayOneShot(GetRandomSound());

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

    private AudioClip GetRandomSound()
    {
        return _ambientSounds[Random.Range(0, _ambientSounds.Length)];
    }

    private void OnCharacterInsideChanged(bool isCharacterInside)
    {
        _isCharacterInsideZone = isCharacterInside;
    }
}