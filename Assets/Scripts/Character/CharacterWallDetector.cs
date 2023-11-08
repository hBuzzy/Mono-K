using UnityEngine;

public class CharacterWallDetector : MonoBehaviour
{
    [SerializeField] private LayerMask _wallMask;
    
    [Header("Left detector")]
    [SerializeField] private Vector3 _leftOffset;
    [SerializeField, Range(0f, 0.3f)] private float _leftRadius;
    
    [Header("Right detector")]
    [SerializeField] private Vector3 _rightOffset;
    [SerializeField, Range(0f, 0.3f)] private float _rightRadius;
    
    private readonly Collider2D[] _collisions = new Collider2D[1];
    
    private bool _isTouchingWall;
    private bool _isTouchingLeft;

    private void Update()
    {
        Vector3 position = transform.position;
        
        bool isTouchingLeft = Physics2D.OverlapCircleNonAlloc(position + _leftOffset,
            _leftRadius, _collisions, _wallMask) > 0;
        bool isTouchingRight = Physics2D.OverlapCircleNonAlloc(position + _rightOffset,
            _rightRadius, _collisions, _wallMask) > 0;

        _isTouchingWall = (isTouchingLeft || isTouchingRight);
        _isTouchingLeft = isTouchingLeft;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = _isTouchingWall ? Color.green : Color.red;

        Vector3 position = transform.position;
        
        Gizmos.DrawWireSphere(position + _leftOffset, _leftRadius);
        Gizmos.DrawWireSphere(position + _rightOffset, _rightRadius);
    }

    public bool IsTouchingWall()
    {
        return _isTouchingWall;
    }

    public bool IsTouchingWall(out bool isTouchingLeft)
    {
        isTouchingLeft = _isTouchingLeft;
        return IsTouchingWall();
    }
}
