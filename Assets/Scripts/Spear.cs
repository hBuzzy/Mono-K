using System;
using System.Collections;
using UnityEngine;

public class Spear : MonoBehaviour
{
    [SerializeField] private float _waitBeforeAttack;
    [SerializeField] private float _waitAfterAttack;
    [SerializeField] private float _attackSpeed;
    [SerializeField] private float _foldSpeed;
    [SerializeField] private AudioSource _audioSource;

    private readonly int _idle = Animator.StringToHash("Idle");
    private readonly int _prepareAttack = Animator.StringToHash("PrepareAttack");
    private readonly int _attack = Animator.StringToHash("Attack");
    private readonly int _shine = Animator.StringToHash("Shine");
    private readonly int _backToNormal = Animator.StringToHash("BackToNormal");
    
    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        StartCoroutine(Live());
    }

    private IEnumerator Live()
    {
        while (enabled)
        {
            yield return StartCoroutine(DoSomething());
        }
    }

    private IEnumerator DoSomething()
    {
        _animator.CrossFade(_idle, 0, 0);
        yield return new WaitForSeconds(1f);
        
        _animator.CrossFade(_prepareAttack, 0, 0);
        yield return new WaitForSeconds(2f);
        
        _audioSource.PlayOneShot(_audioSource.clip);
        _animator.CrossFade(_attack, 0, 0);
        yield return new WaitForSeconds(1f);
        
        _animator.CrossFade(_shine, 0, 0);
        yield return new WaitForSeconds(2f);

        //_animator.CrossFade(_backToNormal, 0, 0);
        //yield return new WaitForSeconds(0.1f);
        
        _animator.CrossFade(_idle, 0, 0);
    }
}
