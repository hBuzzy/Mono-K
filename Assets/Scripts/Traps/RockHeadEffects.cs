using System;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(PathMover))]

public class RockHeadEffects : MonoBehaviour
{
    [SerializeField] private AudioSource _moveSound;
    [SerializeField] private AudioSource _hitSound;

    private Animator _animator;
    private PathMover _pathMover;

    private void Awake()
    {
        _pathMover = GetComponent<PathMover>();
    }

    private void OnEnable()
    {
        _pathMover.Moved += PlayMoveEffects;
    }

    private void OnDisable()
    {
        _pathMover.Moved -= PlayMoveEffects;
    }

    private void PlayMoveEffects()
    {
        _moveSound.PlayOneShot(_moveSound.clip);
    }
}
