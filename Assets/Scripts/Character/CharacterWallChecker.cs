using UnityEngine;

public class CharacterWallChecker : MonoBehaviour
{
    [SerializeField] private LayerMask _wallMask;
    
    [Header("Left")]
    [SerializeField] private Vector3 _leftGrounderOffset;
    [SerializeField, Range(0f, 1f)] private float _leftRadius;
    
    [Header("Left")]
    [SerializeField] private Vector3 _rightGrounderOffset;
    [SerializeField, Range(0f, 1f)] private float _rightRadius;
    
    private readonly Collider2D[] _groundHits = new Collider2D[1];
    private bool _isOnWall = false;

    private void Update()
    {
        _isOnWall = Physics2D.OverlapCircleNonAlloc(transform.position + _leftGrounderOffset, _leftRadius, _groundHits, _wallMask) > 0
            || Physics2D.OverlapCircleNonAlloc(transform.position + _rightGrounderOffset, _rightRadius, _groundHits, _wallMask) > 0;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = _isOnWall ? Color.green : Color.red;
        Gizmos.DrawWireSphere(transform.position + _leftGrounderOffset, _leftRadius);
        Gizmos.DrawWireSphere(transform.position + _rightGrounderOffset, _rightRadius);
    }

    public bool GetOnWall()
    {
        return _isOnWall;
    }
}
