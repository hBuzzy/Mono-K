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
    
    CancellationTokenSource disableCancellation = new CancellationTokenSource();
    CancellationTokenSource destroyCancellation = new CancellationTokenSource();

    private void OnEnable()
    {
        if (disableCancellation != null)
        {
            disableCancellation.Dispose();
        }
        disableCancellation = new CancellationTokenSource();
        Run(disableCancellation);
    }

    private void OnDisable()
    {
        disableCancellation.Cancel();
    }

    private void OnDestroy()
    {
        destroyCancellation.Cancel();
        destroyCancellation.Dispose();
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