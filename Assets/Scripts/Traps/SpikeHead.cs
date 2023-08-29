﻿using UnityEngine;

[RequireComponent(typeof(PathMover))]
[RequireComponent(typeof(Animator))]

public class SpikeHead : MonoBehaviour
{
    [SerializeField] private SpikeHeadHitBoxes _hitBoxes;
    [SerializeField] private AudioSource _moveSound;
    [SerializeField] private AudioSource _hitSound;
    
    private const float AnimationTransitionDuration = 0.1f;

    private readonly int _leftHit = Animator.StringToHash("LeftHit");
    private readonly int _rightHit = Animator.StringToHash("RightHit");
    private readonly int _topHit = Animator.StringToHash("TopHit");
    private readonly int _bottomHit = Animator.StringToHash("BottomHit");

    private PathMover _pathMover;
    private Animator _animator;
    
    private void Awake()
    {
        _pathMover = GetComponent<PathMover>();
        _animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        _hitBoxes.CharacterEntered += HandleHit;
        _pathMover.Moved += OnMove;
    }

    private void OnDisable()
    {
        _hitBoxes.CharacterEntered -= HandleHit;
        _pathMover.Moved -= OnMove;
    }

    private void HandleHit(Sides hitSide)
    {
        if (hitSide == Sides.Left)
        {
            AnimateState(_leftHit);
        }
        else if (hitSide == Sides.Right)
        {
            AnimateState(_rightHit);
        }
        else if (hitSide == Sides.Top)
        {
            AnimateState(_topHit);
        }
        else if (hitSide == Sides.Bottom)
        {
            AnimateState(_bottomHit);
        }
        
        _hitSound.PlayOneShot(_hitSound.clip);
    }

    private void AnimateState(int animationState)
    {
        _animator.CrossFade(animationState, AnimationTransitionDuration);
    }

    private void OnMove()
    {
        _moveSound.PlayOneShot(_moveSound.clip);
    }
}