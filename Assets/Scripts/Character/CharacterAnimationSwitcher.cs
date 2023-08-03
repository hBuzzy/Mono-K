using UnityEngine;
using States = CharacterStates.States;

public class CharacterAnimationSwitcher : MonoBehaviour
{
    [SerializeField] private CharacterStates _states;

    private readonly int _idle = Animator.StringToHash("Idle");
    private readonly int _jump = Animator.StringToHash("Jump");
    private readonly int _fall = Animator.StringToHash("Fall");
    private readonly int _walk = Animator.StringToHash("Walk");
    private readonly int _dashPreparation = Animator.StringToHash("DashPreparation");
    private readonly int _dash = Animator.StringToHash("Dash");
    private readonly int _slide = Animator.StringToHash("Slide");
    private readonly int _hurt = Animator.StringToHash("Hurt");
    private readonly int _grab = Animator.StringToHash("Grab");

    private readonly int _transitionDuration;
    
    private Animator _animator;
    
    private bool _isJumping;
    private bool _isGrounded;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        _states.StateChanged += AnimateState;
    }

    private void OnDisable()
    {
        _states.StateChanged -= AnimateState;
    }

    private void AnimateState(States state)
    {
        var stateAnimation = GetStateAnimation(state);

        _animator.CrossFade(stateAnimation, _transitionDuration, 0);
    }
    
    private int GetStateAnimation(States state)
    {
        if (state == States.Hurt)
        {
            return _hurt;
        }

        if (state == States.Grab)
        {
            return _grab;
        }
        
        if (state == States.Slide)
        {
            return _slide;
        }

        if (state == States.DashPreparation)
        {
            return _dashPreparation;
        }
        
        if (state == States.Dash)
        {
            return _dash;
        }
        
        if (state == States.Jump)
        {
            return _jump;
        }
        
        if (state == States.Move)
        {
            return _walk;
        }
        
        if (state == States.Fall)
        {
            return _fall;
        }
        
        return _idle;
    }
}
