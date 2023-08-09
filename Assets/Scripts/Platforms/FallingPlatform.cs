using System.Collections;
using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CapsuleCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]

public class FallingPlatform : MonoBehaviour
{
    
    [Header("Shaking")] 
    [SerializeField, Range(0f, 7f)] private float _vibratoDuration;
    [SerializeField] private Vector2 _strength;
    [SerializeField, Range(0, 10)] private int _vibrato;
    [SerializeField, Range(0,  90)] private int _randomness;

    [Header("Times")]
    [SerializeField, Range(0f, 3f)] private float _fallTime;
    [SerializeField, Range(0f, 3f)] private float _respawnTime;
    
    [SerializeField] private Brick[] _bricks;
    
    private BoxCollider2D _boxCollider;
    private CapsuleCollider2D _capsuleCollider;
    private Rigidbody2D _rigidbody;

    private Coroutine _currentCoroutine;
    private Vector3 _defaultPositions;
    
    private void Start()
    {
        _boxCollider = GetComponent<BoxCollider2D>();
        _capsuleCollider = GetComponent<CapsuleCollider2D>();
        _rigidbody = GetComponent<Rigidbody2D>();

        _defaultPositions = transform.position;
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out Character character))
        {
            _currentCoroutine ??= StartCoroutine(Fall());
        }
    }

    private IEnumerator Fall()
    {
        ShakeBreaks();

        yield return new WaitForSeconds(_vibratoDuration);

        _boxCollider.enabled = false;
        _capsuleCollider.enabled = false;
        
        yield return DropBricks(_fallTime);

        yield return new WaitForSeconds(_respawnTime);
        
        foreach (var brick in _bricks)
        {
            brick.GetBack();
        }
        
        transform.position = _defaultPositions;
        
        _boxCollider.enabled = true;
        _capsuleCollider.enabled = true;
        
        _currentCoroutine = null;
    }

    private void ShakeBreaks()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            _bricks[i].transform.DOShakePosition(_vibratoDuration, (Vector3)_strength, _vibrato, _randomness);
        }
    }

    private IEnumerator DropBricks(float duration)
    {
        _rigidbody.isKinematic = false;

        foreach (var brick in _bricks)
        {
            brick.Fall();
        }

        yield return new WaitForSeconds(duration);

        _rigidbody.isKinematic = true;
        _rigidbody.velocity = Vector2.zero;
    }
}
