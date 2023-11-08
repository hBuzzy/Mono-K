using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class Spears : MonoBehaviour
{
    [SerializeField] private AudioSource _attackSound;
    [SerializeField, Range(0f, 5f)] private float _waitBeforeStart;
    [SerializeField, Range(0f, 5f)] private float _waitBeforeAttack;

    [Header("Spears")]
    [SerializeField] private Spear[] _spears;
    
    //TODO:
    CancellationTokenSource _disableCancellation = new ();
    CancellationTokenSource _destroyCancellation = new ();

    private void OnEnable()
    {
        if (_disableCancellation != null)
        {
            _disableCancellation.Dispose();
        }
        _disableCancellation = new CancellationTokenSource();
        Run(_disableCancellation);
    }

    private void OnDisable()
    {
        _disableCancellation.Cancel();
    }

    private void OnDestroy()
    {
        _destroyCancellation.Cancel();
        _destroyCancellation.Dispose();
    }

    private async void Run(CancellationTokenSource tokenSource)
    {
        UniTask[] tasks = new UniTask[_spears.Length];

        await UniTask.Delay(TimeSpan.FromSeconds(_waitBeforeStart));

        while (tokenSource.IsCancellationRequested == false)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(_waitBeforeAttack));

            if (tokenSource.IsCancellationRequested == false)
            {
                _attackSound.PlayOneShot(_attackSound.clip);

                for (int i = 0; i < _spears.Length; i++)
                {
                    tasks[i] = _spears[i].Attack();
                }
            }

            await UniTask.WhenAll(tasks);    
        }
    }
}