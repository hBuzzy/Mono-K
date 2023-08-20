using DG.Tweening;
using UnityEngine;

public class Door : MonoBehaviour
{
    [Header("Points")]
    [SerializeField] private Transform _openedPoint;
    [SerializeField] private Transform _closePoint;
    
    [Header("Settings")]
    [SerializeField] private AudioSource _openSound;
    [SerializeField] private float _moveDuration;

    private Vector3 _currentPosition;

    public void Open()
    {
        transform.DOMove(_openedPoint.position, _moveDuration);
        _openSound.PlayOneShot(_openSound.clip);
    }

    public void Close()
    {
        transform.DOMove(_openedPoint.position, _moveDuration);
        _openSound.PlayOneShot(_openSound.clip);
    }
}
