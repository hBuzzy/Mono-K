using System;
using UnityEngine;

public class CharacterAnimationSwitcher : MonoBehaviour
{
    private readonly int _idle = Animator.StringToHash("Idle");
    private readonly int _jump = Animator.StringToHash("Jump");

    private int _currentState;
    
    private Animator _animator;

    private CharacterJump _characterJump;

    private bool _isJumping;
    
    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _characterJump = GetComponent<CharacterJump>();
    }

    private void OnEnable()
    {
        _characterJump.Jumped += () => { _isJumping = true; };
    }

    private void OnDisable()
    {
        
    }

    private void Update()
    {
        var state = GetState();
        
        if (state == _currentState)
            return;
        
        ResetVariables();
        
        _animator.CrossFade(state, 0, 0);
        _currentState = state;
    }

    private int GetState()
    {
        if (_isJumping)
        {
            return _jump;
        }

        return _idle;
    }

    private void ResetVariables()
    {
        _isJumping = false;
    }
}
