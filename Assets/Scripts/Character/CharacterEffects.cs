using UnityEngine;
using UnityEngine.Serialization;

public class CharacterEffects : MonoBehaviour
{
    [Header("Components")] 
    [SerializeField] private CharacterStates _characterStates;
    
    [Header("Jump")] 
    [SerializeField] private AudioSource _jumpSound;
    [SerializeField] private ParticleSystem _jumpDustEffect;
    
    [Header("Dash")]
    [SerializeField] private AudioSource _dashSound;
    [FormerlySerializedAs("_dashDust")] [SerializeField] private ParticleSystem _dashDustEffect;

    [Header("Walk")]
    [SerializeField] private AudioSource _walkSound;
    
    private CharacterMovement _characterMovement;
    private CharacterDash _characterDash;
    private CharacterJump _characterJump;
    private Animator _animator;

    private void Awake()
    {
        _characterJump = GetComponent<CharacterJump>();
        _characterDash = GetComponent<CharacterDash>();
        _characterStates.StateChanged += HandleState;
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

    private void HandleState(CharacterStates.States state)
    {
        if (state == CharacterStates.States.Jump)
        {
            PlayJumpEffects();
        }
        else if (state == CharacterStates.States.Dash)
        {
            PlayDashEffect();
        }
    }

    private void PlayJumpEffects()
    {
        _jumpSound.Play();
        _jumpDustEffect.Play();
    }

    private void PlayDashEffect()
    {
        _dashSound.Play();
        _dashDustEffect.Play();
    }
    
    private void OnJumped()
    {
        //_jumpSound.Play();
        //_jumpDustEffect.Play();
    }

    private void OnDash()
    {
        //_dashSound.Play();
        //_dashDustEffect.Play();
    }
}
