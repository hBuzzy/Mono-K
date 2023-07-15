using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]

public class CharacterHurt : MonoBehaviour
{
    [SerializeField] private CharacterRespawner _respawner;
    
    private Rigidbody2D _rigidbody;
    private Character _character;

    private Coroutine _currentCoroutine;

    public event Action<bool> Hurting; 

    private void Awake()
    {
            
    }

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _character = GetComponent<Character>();
    }

    private void OnEnable()
    {
        
    }

    private void OnDisable()
    {
        
    }

    public void OnHurt()
    {
        if (_currentCoroutine == null)
        {
            _currentCoroutine = StartCoroutine(Hurt());
        }
    }

    private IEnumerator Hurt()
    {
        Hurting?.Invoke(true);
        
        _rigidbody.velocity = Vector2.zero;
        _rigidbody.simulated = false;
        
        yield return new WaitForSeconds(1);
        
        Hurting?.Invoke(false);
        
        _respawner.MoveToCheckPoint(_character);
        
        _rigidbody.simulated = true;

        _currentCoroutine = null;
    }
}