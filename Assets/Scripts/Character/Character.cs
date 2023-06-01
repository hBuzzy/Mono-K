using System;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] private CharacterGround _grounder;

    private bool _isGrounded;
    private bool _isCurrentlyGrounded;
    
    public event Action<bool> GrounedChanged;

    private void Start()
    {
        _isGrounded = false;
        _isCurrentlyGrounded = false;
    }

    private void Update()
    {
        _isCurrentlyGrounded = _grounder.GetOnGround();

        if (_isGrounded != _isCurrentlyGrounded)
        {
            _isGrounded = _isCurrentlyGrounded;
            GrounedChanged?.Invoke(_isGrounded);   
        }
    }
}
