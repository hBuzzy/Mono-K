using DG.Tweening;
using UnityEngine;

public class Door : MonoBehaviour
{
    [Header("Points")]
    [SerializeField] private Transform _openPoint;
    [SerializeField] private Transform _closePoint;
    
    [Header("Settings")]
    [SerializeField] private AudioSource _moveSound;
    [SerializeField] private float _moveDuration;

    private Vector3 _currentPosition;

    public void Open()
    {
        transform.DOMove(_openPoint.position, _moveDuration);
        _moveSound.PlayOneShot(_moveSound.clip);
    }

    public void Close()
    {
        transform.DOMove(_closePoint.position, _moveDuration);
        _moveSound.PlayOneShot(_moveSound.clip);
    }
}
