using DG.Tweening;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private Transform _openedPoint;
    [SerializeField] private AudioSource _openSound;
    
    public void Open()
    {
        transform.DOMove(_openedPoint.position, 3f);
        _openSound.PlayOneShot(_openSound.clip);
    }
}
