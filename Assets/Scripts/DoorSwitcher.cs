using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(BoxCollider2D))]
public class DoorSwitcher : MonoBehaviour
{
    private readonly int _switch = Animator.StringToHash("Switch");
    
    private BoxCollider2D _boxCollider;
    private Animator _animator;

    [SerializeField] private UnityEvent Switched;

    private void Start()
    {
        _boxCollider = GetComponent<BoxCollider2D>();
        _animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_boxCollider.enabled == true && collision.TryGetComponent(out Character character))
        {
            _animator.CrossFade(_switch, 0, 0);
            _boxCollider.enabled = false;
            Switched?.Invoke();
        }
    }
}
