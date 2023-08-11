using System.Threading.Tasks;
using UnityEngine;

public class Spear : MonoBehaviour
{
    [SerializeField, Range(0f, 1.5f)] private float _shineTime;

    private readonly int _attack = Animator.StringToHash("Attack");
    private readonly int _backToNormal = Animator.StringToHash("BackToIdle");
    
    private Animator _animator;
    private Coroutine _attackCoroutine;
    
    private float _waitAfterAttack;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public async Task Attack()
    {
        AnimateState(_attack);
        
        await AsyncExtensions.WaitForSeconds(_shineTime);
        
        AnimateState(_backToNormal);
    }

    private void AnimateState(int stateHash, float transitionDuration = 0.1f,int layer = 0)
    {
        _animator.CrossFade(stateHash, transitionDuration, layer);
    }
}
