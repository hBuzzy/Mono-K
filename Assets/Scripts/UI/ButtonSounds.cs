using UnityEngine;

public class ButtonSounds : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _pressedSound;
    [SerializeField] private AudioClip _hoverSound;

    private void Start()
    {
        _audioSource.ignoreListenerPause = true;
    }

    public void PlayClickSound()
    {
        _audioSource.PlayOneShot(_pressedSound);
    }

    public void PlayHoverSound()
    {
        _audioSource.PlayOneShot(_hoverSound);
    }
}