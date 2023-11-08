using UnityEngine;

[RequireComponent(typeof(AudioSource))]

public class UISounds : MonoBehaviour
{
    private AudioSource _audioSource;
    
    public static UISounds Instance { get; private set; }
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _audioSource.ignoreListenerPause = true;
    }

    public void PlaySound(AudioClip clip)
    {
        if (_audioSource.isPlaying)
            _audioSource.Stop();
        
        _audioSource.PlayOneShot(clip);
    }
}