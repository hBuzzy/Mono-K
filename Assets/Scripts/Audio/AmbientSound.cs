using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]

public class AmbientSound : MonoBehaviour
{
    [Header("Sound")]
    [SerializeField] private AudioClip _sound;
    [SerializeField, Range(0f, 1f)] private float _maxVolume;
    
    [Header("Options")]
    [SerializeField] private SoundZone _soundZone;
    [SerializeField, Range(0f, 1f)] private float _volumeChangeSpeed = 0.5f;

    private const float MinVolume = 0f;

    private AudioSource _audioSource;
    private Coroutine _coroutine;
    
    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _audioSource.clip = _sound;
        _audioSource.volume = MinVolume;
    }

    private void OnEnable()
    {
        _soundZone.CharacterInsideChanged += OnCharacterInsideChanged;
    }

    private void OnDisable()
    {
        _soundZone.CharacterInsideChanged -= OnCharacterInsideChanged;
    }

    private void OnCharacterInsideChanged(bool isCharacterInside)
    {
        if (_coroutine != null) 
            StopCoroutine(_coroutine);

        _coroutine = StartCoroutine(isCharacterInside ? SetVolume(_maxVolume) : SetVolume(MinVolume));
    }

    private IEnumerator SetVolume(float volume)
    {
        if (_audioSource.isPlaying == false)
            _audioSource.Play();

        yield return ChangeValueSmoothly(volume);
        
        if (_audioSource.volume == MinVolume)
            _audioSource.Stop();
    }
    
    private IEnumerator ChangeValueSmoothly(float targetValue)
    {
        while (_audioSource.volume != targetValue)
        {
            _audioSource.volume = Mathf.MoveTowards(_audioSource.volume, targetValue, _volumeChangeSpeed * Time.deltaTime);
            yield return null;
        }
    }
}