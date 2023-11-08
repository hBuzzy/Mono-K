using System;
using System.Collections;
using UnityEngine;

public class CharacterDeath : MonoBehaviour
{
    [SerializeField] private EndAnimationNotifier _deathAnimationNotifier;
    
    private Coroutine _dieCoroutine;
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

    private void OnEnable()
    {
        _deathAnimationNotifier.Ended += OnDeathAnimationEnded;
    }

    private void OnDisable()
    {
        _deathAnimationNotifier.Ended -= OnDeathAnimationEnded;
    }

    public void TakeDamage()
    {
        if (_dieCoroutine != null)
            StopCoroutine(_dieCoroutine);
        
        _dieCoroutine = StartCoroutine(Die());
    }

    private IEnumerator Die()
    {
        DeathChanged?.Invoke(true);

        _deathAnimationFinished = false;
        DeathsCount++;

        while (_deathAnimationFinished == false)
            yield return null;

        yield return CharacterRespawner.Instance.RespawnCharacter();
        
        DeathChanged?.Invoke(false);

        _dieCoroutine = null;
    }

    private void OnDeathAnimationEnded()
    {
        _deathAnimationFinished = true;
    }
}