using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]

public class Brick : MonoBehaviour
{
    [SerializeField] private float _animationDuration;

    private const float FadeInValue = 1;
    private const float FadeOutValue = 0;
    
    private readonly int _crash = Animator.StringToHash("Crash");
    private readonly int _idle = Animator.StringToHash("Idle");
    
    private const float AnimationTransitionDuration = 0.1f;

    private Animator _animator;
    private SpriteRenderer _spriteRenderer;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Fall()
    {
        _animator.CrossFade(_crash, AnimationTransitionDuration);
        _spriteRenderer.DOFade(FadeOutValue, _animationDuration);
    }

    public void GetBack()
    {
        _animator.CrossFade(_idle, AnimationTransitionDuration);
        _spriteRenderer.DOFade(FadeInValue, _animationDuration);
    }
}