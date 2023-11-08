using UnityEngine;

public class ButtonSounds : MonoBehaviour
{
    [SerializeField] private AudioClip _clickSound;
    [SerializeField] private AudioClip _hoverSound;

    public AudioClip GetHoverSound()
    {
        return _hoverSound;
    }

    public AudioClip GetClickSound()
    {
        return _clickSound;
    }
}