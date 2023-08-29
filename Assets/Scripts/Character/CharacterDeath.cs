using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Character))]

public class CharacterDeath : MonoBehaviour
{
    private Animator _animator;
    private Coroutine _currentCoroutine;
    private int _deathsCount;
    private bool _deathAnimationFinished;

    public int DeathsCount
    {
        get => _deathsCount;
        private set
        {
            _deathsCount = value;
            DeathsCountChanged?.Invoke(_deathsCount);
        }
    }
    
    public event Action<bool> DeathChanged;
    public event Action<int> DeathsCountChanged;

    public void TakeDamage()
    {
        if (_currentCoroutine != null)
            StopCoroutine(_currentCoroutine);
        
        _currentCoroutine = StartCoroutine(Die());
    }

    private IEnumerator Die()
    {
        DeathChanged?.Invoke(true);

        _deathAnimationFinished = false;
        DeathsCount++;

        while (_deathAnimationFinished == false)
        {
            yield return null;
        }

        yield return CharacterRespawner.Instance.RespawnCharacter();
        
        DeathChanged?.Invoke(false);

        _currentCoroutine = null;
    }

    private void OnDeathAnimationFinished()
    {
        _deathAnimationFinished = true;
    }
}