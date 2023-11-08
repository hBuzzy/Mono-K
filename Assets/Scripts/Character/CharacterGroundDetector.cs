using UnityEngine;

public class CharacterGroundDetector : MonoBehaviour
{
    private bool _isGrounded;

    public bool IsGrounded => _isGrounded;

    [Header("Collider Settings")] 
    [SerializeField] private float _colliderLength;
    [SerializeField] private Vector3 _colliderOffset;

    [Header("Layer Masks")]
    [SerializeField] private LayerMask _groundLayer;

    private void Update()
    {
        Vector3 position = transform.position;
        
        _isGrounded = Physics2D.Raycast(position + _colliderOffset, Vector2.down, _colliderLength, _groundLayer)
                      || Physics2D.Raycast(position - _colliderOffset, Vector2.down, _colliderLength, _groundLayer);
    }

    private void OnDrawGizmos() {
        Gizmos.color = _isGrounded ? Color.green : Color.red;

        Vector3 position = transform.position;
        
        Gizmos.DrawLine(position + _colliderOffset, 
            position + _colliderOffset + Vector3.down * _colliderLength);
        
        Gizmos.DrawLine(position - _colliderOffset,
            position - _colliderOffset + Vector3.down * _colliderLength);
    }
}
