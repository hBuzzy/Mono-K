using System.Collections;
using Cinemachine;
using DG.Tweening;
using UnityEngine;

public class CharacterEffects : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Camera _cameraMain;
    [SerializeField] private CinemachineVirtualCamera _camera;
    [SerializeField] private CharacterStates _characterStates;

    [Header("Jump")] 
    [SerializeField] private AudioSource _jumpSound;
    [SerializeField] private ParticleSystem _jumpDustEffect;

    [Header("Dash")]
    [SerializeField] private AudioSource _dashSound;
    [SerializeField] private ParticleSystem _dashDustEffect;
    [SerializeField, Range(0f, 15f)] private float _cameraShakeIntensity;
    [SerializeField, Range(0, 1f)] private float _cameraShakeTime;

    [SerializeField] private RippleEffect _rippleEffect;

    [Header("Ghost trail")]
    [SerializeField] private bool _isTrailEnable;
    [SerializeField, Range(2, 10)] private int _ghostsCount;
    [SerializeField, Range(0f, 1f)] private float _trailDuration;

    [Header("Hurt")]
    [SerializeField] private AudioSource _hitSound;
    
    private CharacterMovement _characterMovement;
    private CharacterDash _characterDash;
    private CharacterJump _characterJump;
    private Animator _animator;

    private Coroutine _shakeCoroutine;
    private Vector3 _characterLastPosition;
    
    private void Awake()
    {
        _characterJump = GetComponent<CharacterJump>();
        _characterDash = GetComponent<CharacterDash>();
    }

    private void OnEnable()
    {
        _characterJump.Jumped += OnJumped;
        _characterDash.Dashed += OnDash;
        _characterStates.StateChanged += HandleState;
    }

    private void OnDisable()
    {
        _characterJump.Jumped -= OnJumped;
        _characterDash.Dashed -= OnDash;
    }

    private void HandleState(CharacterStates.States state)
    {
        //_walkSound.Stop();
        
        if (state == CharacterStates.States.Jump)
        {
            //PlayJumpEffects();
        }
        else if (state == CharacterStates.States.Dash)
        {
            PlayDashEffect();
        }
        else if (state == CharacterStates.States.Move)
        {
            //_walkSound.Play();
        }
        else if (state == CharacterStates.States.Hurt)
        {
            PlayHurtEffect();
        }
    }

    private void PlayJumpEffects()
    {
        _jumpSound.Play();
        _jumpDustEffect.Play();
    }

    private void PlayDashEffect()
    {
        _dashSound.PlayOneShot(_dashSound.clip);
        _dashDustEffect.Play();
        _rippleEffect.Show();
        
        StartCoroutine(ShakeCamera());
        
        if (_isTrailEnable)
        {
            StartCoroutine(ShowGhostTrail());
        }
    }

    private void PlayHurtEffect()
    {
        _hitSound.PlayOneShot(_hitSound.clip);
    }

    private void OnJumped()
    {
        _jumpSound.Play();
        _jumpDustEffect.Play();
    }

    private void OnDash()
    {
        //_dashSound.Play();
        //_dashDustEffect.Play();
    }

    private IEnumerator ShakeCamera()//TODO: Need rafactor
    {
        CinemachineBasicMultiChannelPerlin _perlin =
            _camera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        _perlin.m_AmplitudeGain = _cameraShakeIntensity;

        yield return new WaitForSeconds(_cameraShakeTime);

        _perlin.m_AmplitudeGain = 0f;
    }
    
    private IEnumerator ShowGhostTrail()//TODO: Make performance tests
    {
        float delayBetweenGhosts = _trailDuration / (_ghostsCount - 1);
        
        GhostTrailPool.Instance.GetGhost().Show();
        
        for (int i = 0; i < _ghostsCount - 1; i++)
        {
            yield return new WaitForSeconds(delayBetweenGhosts);
            GhostTrailPool.Instance.GetGhost().Show();
        }
    }
}
