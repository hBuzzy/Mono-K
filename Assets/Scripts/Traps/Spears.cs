using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class Spears : MonoBehaviour
{
    [SerializeField] private AudioSource _attackSound;
    [SerializeField, Range(0f, 2f)] private float _waitBeforeStart;
    [SerializeField, Range(0f, 5f)] private float _waitBeforeAttack;

    [Header("Spears")]
    [SerializeField] private Spear[] _spears;

    private CancellationTokenSource _cancelTokenSource;
    private CancellationToken _token;
    
    private void Awake()
    {
        _cancelTokenSource = new CancellationTokenSource();
        _token = _cancelTokenSource.Token;
    }

    private void OnEnable()
    { 
        Run(_token);
    }

    private void OnDisable()
    {
        _cancelTokenSource.Cancel();
    }

    private void OnDestroy()
    {
        _cancelTokenSource.Cancel();
    }

    private async void Run(CancellationToken token)
    {
        Task[] tasks = new Task[_spears.Length];

        await AsyncExtensions.WaitForSeconds(_waitBeforeStart);

        while (token.IsCancellationRequested == false)
        {
            await AsyncExtensions.WaitForSeconds(_waitBeforeAttack);
            
            _attackSound.PlayOneShot(_attackSound.clip);

            for (int i = 0; i < _spears.Length; i++)
            {
                tasks[i] = _spears[i].Attack();
            }

            await Task.WhenAll(tasks);
        }
    }
}