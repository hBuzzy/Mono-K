using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class Spears : MonoBehaviour
{
    [SerializeField, Range(0f, 5f)] private float _waitBeforeStart;
    [SerializeField, Range(0f, 5f)] private float _waitBeforeAttack;

    [Header("Spears")]
    [SerializeField] private Spear[] _spears;
    
    private AudioSource _attackSound;
    private CancellationToken _cancellationToken;
    
    private void Start()
    {
        _cancellationToken = new CancellationToken();
        _attackSound = GetComponent<AudioSource>();

        Run(_cancellationToken);
    }

    private async void Run(CancellationToken token)
    {
        UniTask[] tasks = new UniTask[_spears.Length];

        await UniTask.Delay(TimeSpan.FromSeconds(_waitBeforeStart), cancellationToken: token);

        while (token.IsCancellationRequested == false)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(_waitBeforeAttack), cancellationToken: token);

            _attackSound.PlayOneShot(_attackSound.clip);

            for (int i = 0; i < _spears.Length; i++)
            {
                tasks[i] = _spears[i].Attack(token);
            }

            await UniTask.WhenAll(tasks);    
        }
    }
}