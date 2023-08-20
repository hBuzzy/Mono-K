using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(BoxCollider2D))]
public class DoorLever : MonoBehaviour//TODO: Rename switcher to lifter or somehow in this way?
{
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
            _animator.CrossFade(_switch, 0, 0);
            _boxCollider.enabled = false;
            _switched?.Invoke();
        }
    }
}
