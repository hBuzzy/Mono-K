using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Character))]

public class CharacterHurt : MonoBehaviour
{
    private Rigidbody2D _rigidbody;
    private Character _character;

    private Coroutine _currentCoroutine;

    public event Action<bool> HurtingChanged; 

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

    public void Die()
    {
        if (_currentCoroutine == null)
        {
            _currentCoroutine = StartCoroutine(Hurt());
        }
    }

    private IEnumerator Hurt()//TODO: refactor
    {
        HurtingChanged?.Invoke(true);
        
        //_rigidbody.velocity = Vector2.zero; //FIX IT cuz char moving
        _rigidbody.simulated = false;
        
        yield return new WaitForSeconds(1);//TODO: Magic Number
        
        HurtingChanged?.Invoke(false);
        
        CharacterRespawner.Instance.MoveCharacterToCheckPoint();
        
        _rigidbody.simulated = true;

        _currentCoroutine = null;
    }
}