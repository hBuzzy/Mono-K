using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Animator))]

public class Fireball : MonoBehaviour
{
    [SerializeField] private AudioSource _explosionSound;
    [SerializeField] private float _respawnAfterTime;

    //TODO: Replace empty idle with Animation events?
    //TODO: Create state machine ?
    
    private readonly int _move = Animator.StringToHash("Move");
    private readonly int _explosion = Animator.StringToHash("Explosion");

    private Animator _animator;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _animator.CrossFade(_move, 0.1f, 0);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.TryGetComponent(out CharacterHurt character))
        {
            character.Die();
            StartCoroutine(Explosion());
        }
    }

    private IEnumerator Explosion()
    {
        _animator.CrossFade(_explosion, 0.1f, 0);
        yield return new WaitForSeconds(1f);
        
        gameObject.SetActive(false);

        yield return new WaitForSeconds(2f);
        gameObject.SetActive(true);
        
        _animator.CrossFade(_move, 0.1f, 0);
    }
}
