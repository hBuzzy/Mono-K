using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Animator))]

public class Cake : MonoBehaviour
{
    [SerializeField] private AudioSource _takingSound;

    private const float AnimationTransitionDuration = 0.1f;
    
    private readonly int _disappear = Animator.StringToHash("Disappear");

    private Animator _animator;
    private bool _isDisappearFinished;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out PieBasket pieBasket))
        {
            pieBasket.AddPie();
            StartCoroutine(Disappear());
        }
    }

    private void OnDisappearFinished()
    {
        _isDisappearFinished = true;
    }

    private IEnumerator Disappear()
    {
        _takingSound.PlayOneShot(_takingSound.clip);

        _isDisappearFinished = false;
        _animator.CrossFade(_disappear, AnimationTransitionDuration);

        while (_takingSound.isPlaying || _isDisappearFinished == false)
        {
            yield return null;
        }
        
        gameObject.SetActive(false);
    }
}
