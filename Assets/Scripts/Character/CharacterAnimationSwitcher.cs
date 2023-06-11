using UnityEngine;

public class CharacterAnimationSwitcher : MonoBehaviour
{
    [SerializeField] private CharacterStates _states;
    
    private readonly int _idle = Animator.StringToHash("Idle");
    private readonly int _jumpStart= Animator.StringToHash("JumpStart");
    private readonly int _jump = Animator.StringToHash("Jump");
    private readonly int _fall = Animator.StringToHash("Fall");
    private readonly int _jumpEnd = Animator.StringToHash("JumpEnd");
    private readonly int _walk = Animator.StringToHash("Walk");
    private readonly int _dash = Animator.StringToHash("Dash");
    
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

    private void AnimateState(CharacterStates.States state)
    {
        var stateAnimation = GetStateAnimation(state);
        
        //ResetVariables(state);
        
        _animator.CrossFade(stateAnimation, 0, 0);
    }
    
    private int GetStateAnimation(CharacterStates.States state)
    {

        if (state == CharacterStates.States.Dash)
        {
            return _dash;
        }
        
        if (state == CharacterStates.States.Jump)
        {
            return _jump;
        }
        
        
        if (state == CharacterStates.States.Move)
        {
            return _walk;
        }
        
        if (state == CharacterStates.States.Fall)
        {
            return _fall;
        }
        
        return _idle;
    }

    private void ResetVariables(int newState)
    {

    }
}
