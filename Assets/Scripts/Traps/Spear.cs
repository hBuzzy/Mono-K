using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class Spear : MonoBehaviour
{
    [SerializeField, Range(0f, 1.5f)] private float _shineTime;

    private readonly int _attack = Animator.StringToHash("Attack");
    private readonly int _backToNormal = Animator.StringToHash("BackToIdle");
    
    private Animator _animator;
    private Coroutine _attackCoroutine;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public async UniTask Attack(CancellationToken cancellationToken)
    {
        AnimateState(_attack);

        await UniTask.Delay(TimeSpan.FromSeconds(_shineTime), cancellationToken: cancellationToken);
        
        AnimateState(_backToNormal);
    }

    private void AnimateState(int stateHash, float transitionDuration = 0f)
    {
        _animator.CrossFade(stateHash, transitionDuration);
    }
}
