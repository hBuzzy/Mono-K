using System;
using UnityEngine;

public class CharacterEffects : MonoBehaviour
{
    [Header("Jump")] 
    [SerializeField] private AudioSource _jumpSound;
    
    [Header("Dash")]
    [SerializeField] private AudioSource _dashSound;

    [Header("Walk")]
    [SerializeField] private AudioSource _walkSound;
    
    private CharacterMovement _characterMovement;
    private CharacterDash _characterDash;
    private CharacterJump _characterJump;
    
    private void Awake()
    {
        _characterJump = GetComponent<CharacterJump>();
        _characterDash = GetComponent<CharacterDash>();
    }

    private void OnEnable()
    {
        _characterJump.Jumped += OnJumped;
        _characterDash.Dashed += OnDash;
    }

    private void OnDisable()
    {
        _characterJump.Jumped -= OnJumped;
        _characterDash.Dashed -= OnDash;
    }

    private void Update()
    {
        
    }
    
    private void OnJumped()
    {
        _jumpSound.Play();
    }

    private void OnDash()
    {
        _dashSound.Play();
    }
}
