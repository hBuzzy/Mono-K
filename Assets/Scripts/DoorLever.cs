using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(BoxCollider2D))]

public class DoorLever : MonoBehaviour
{
    private const float AnimationTransitionDuration = 0.1f;
    
    private readonly int _switch = Animator.StringToHash("Switch");
    
    private BoxCollider2D _boxCollider;
    private Animator _animator;

    [SerializeField] private UnityEvent _switched;

    private void Start()
    {
        _boxCollider = GetComponent<BoxCollider2D>();
        _animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_boxCollider.enabled && collision.TryGetComponent(out Character character))
        {
            _boxCollider.enabled = false;
            _animator.CrossFade(_switch, AnimationTransitionDuration);
            _switched?.Invoke();
        }
    }
}
