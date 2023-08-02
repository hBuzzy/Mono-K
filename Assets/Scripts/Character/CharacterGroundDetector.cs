using UnityEngine;
using UnityEngine.Serialization;

public class CharacterGroundDetector : MonoBehaviour
{
    private bool _isGrounded;

    public bool IsGrounded => _isGrounded;
    
    [FormerlySerializedAs("groundLength")]
    [Header("Collider Settings")]
    [SerializeField][Tooltip("Length of the ground-checking collider")] private float _groundLength = 0.95f;
    [FormerlySerializedAs("colliderOffset")] [SerializeField][Tooltip("Distance between the ground-checking colliders")] private Vector3 _colliderOffset;

    [Header("Layer Masks")]
    [SerializeField][Tooltip("Which layers are read as the ground")] private LayerMask groundLayer;


    private void Update() {
        //Determine if the player is stood on objects on the ground layer, using a pair of raycasts
        _isGrounded = Physics2D.Raycast(transform.position + _colliderOffset, Vector2.down, _groundLength, groundLayer) || Physics2D.Raycast(transform.position - _colliderOffset, Vector2.down, _groundLength, groundLayer);
    }

    private void OnDrawGizmos() {
        //Draw the ground colliders on screen for debug purposes
        if (_isGrounded) { Gizmos.color = Color.green; } else { Gizmos.color = Color.red; }
        Gizmos.DrawLine(transform.position + _colliderOffset, transform.position + _colliderOffset + Vector3.down * _groundLength);
        Gizmos.DrawLine(transform.position - _colliderOffset, transform.position - _colliderOffset + Vector3.down * _groundLength);
    }
}
