using System.Collections;
using DG.Tweening;
using UnityEngine;

public class FallingPlatform : MonoBehaviour
{

    [Header("Shaking")] 
    [SerializeField, Range(0f, 7f)] private float _duration;
    [SerializeField] private Vector2 _strength;
    [SerializeField, Range(0, 10)] private int _vibrato;
    [SerializeField, Range(0,  90)] private int _randomness;

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
            if (_currentCoroutine == null)
            {
                StartCoroutine(Fall());
            }
        }
    }

    private IEnumerator Fall()
    {
        ShakeBreaks();

        yield return new WaitForSeconds(2f);

        _boxCollider.enabled = false;
        _capsuleCollider.enabled = false;
        
        yield return DropBricks(2f);

        yield return new WaitForSeconds(2f);
        
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
            _bricks[i].transform.DOShakePosition(_duration, (Vector3)_strength, _vibrato, _randomness, false, true);
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
