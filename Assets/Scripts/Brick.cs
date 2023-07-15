using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]

public class Brick : MonoBehaviour
{
    private readonly int _crash = Animator.StringToHash("Crash");
    private readonly int _idle = Animator.StringToHash("Idle");

    private Animator _animator;
    private SpriteRenderer _spriteRenderer;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Fall()
    {
        _animator.CrossFade(_crash, 0, 0);
        _spriteRenderer.DOFade(0f, 0.5f);
    }

    public void GetBack()
    {
        _animator.CrossFade(_idle, 0, 0);
        _spriteRenderer.DOFade(1f, 0.5f);
    }
}
