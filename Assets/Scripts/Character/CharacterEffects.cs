using UnityEngine;
using States = CharacterData.States;

[RequireComponent(typeof(AudioSource))]

public class CharacterEffects : MonoBehaviour
{
    [Header("Components")] 
    [SerializeField] private CharacterDash _dash;
    
    [Header("VFX")]
    [SerializeField] private CameraShaker _cameraShaker;
    [SerializeField] private ParticleSystem _jumpDust;
    [SerializeField] private ParticleSystem _dashDust;
    [SerializeField] private DashRipple _dashRipple;
    [SerializeField] private ShockWave _shockWave;
    [SerializeField] private CharacterGhostTrail _ghostTrail;
    [SerializeField] private CharacterOutline _characterOutline;
    [SerializeField] private Squeeze _jumpSqueeze;
    
    [Header("Sounds")] 
    [SerializeField] private AudioClip _jumpSound;
    [SerializeField] private AudioClip _dashSound;
    [SerializeField] private AudioClip _dieSound;
    
    private const States Die = States.Die;
    private const States Jump = States.Jump;
    private const States Dash = States.Dash;

    private AudioSource _audioSource;
    private Coroutine _shakeCoroutine;
    private Vector3 _characterLastPosition;

    public void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        _dash.CanDashChanged += OnCanDashChanged;
    }

    private void OnDisable()
    {
        _dash.CanDashChanged -= OnCanDashChanged;
    }

    public void Play(States state)
    {
        switch (state)
        {
            case Jump:
                PlayJumpEffects();
                break;
            
            case Dash:
                PlayDashEffects();
                break;
            
            case Die:
                PlayHurtEffects();
                break;
        }
    }

    private void PlayJumpEffects()
    {
        _audioSource.PlayOneShot(_jumpSound);
        _jumpDust.Play();
        _jumpSqueeze.Play();
    }

    private void PlayDashEffects()
    {
        _audioSource.PlayOneShot(_dashSound);
        _dashDust.Play();
        _dashRipple.Play(transform.position);
        _shockWave.Show();
        _cameraShaker.Shake();
        _ghostTrail.Show();
    }
    
    private void PlayHurtEffects()
    {
        _audioSource.PlayOneShot(_dieSound);
    }

    private void OnCanDashChanged(bool canDash)
    {
        if (canDash)
            _characterOutline.Show();
        else
            _characterOutline.Hide();
    }
}