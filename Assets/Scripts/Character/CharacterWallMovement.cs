using System;
using UnityEngine;

public class CharacterWallMovement : MonoBehaviour
{
    private Character _character;
    private Rigidbody2D _rigidbody;
    private bool _isOnWall;
    private bool _isGrounded;
    private bool _isSliding;
    private bool _isSlidingRem;

    public event Action<bool> SlidingStatusChanged; 

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _character = GetComponent<Character>();
    }
    
    private void OnEnable()
    {
        _character.WalledChanged += isOnWall => { _isOnWall = isOnWall; };
        _character.GroundedChanged += isGrounded => { _isGrounded = isGrounded; };
    }

    private void Update()
    {
        // if (_isOnWall && _rigidbody.velocity.y < 0)
        // {
        //     _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, 0);
        // }
        
        if (_isOnWall && _isGrounded == false && _character.DirectionX != 0)
        {
            _isSliding = true;
        }
        else
        {
            _isSliding = false;
        }

        if (_isSliding != _isSlidingRem)
        {
            _isSlidingRem = _isSliding;
            SlidingStatusChanged.Invoke(_isSliding);
        }

        if (_isSliding)
        {
            _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, Mathf.Clamp(_rigidbody.velocity.y, -0.7f, float.MaxValue));
        }
    }
}