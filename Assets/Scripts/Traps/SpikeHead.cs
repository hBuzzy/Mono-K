using UnityEngine;

[RequireComponent(typeof(PathMover))]

public class SpikeHead : MonoBehaviour //TODO: Need it?
{
    [SerializeField] private SpikeHeadHitBoxes _hitBoxes;
    [SerializeField] private AudioSource _moveSound;

    private PathMover _pathMover;
    
    private void Awake()
    {
        _pathMover = GetComponent<PathMover>();
    }

    private void OnEnable()
    {
        _hitBoxes.CharacterEntered += HandleHit;
        _pathMover.Moved += OnMove;
    }

    private void OnDisable()
    {
        _hitBoxes.CharacterEntered -= HandleHit;
        _pathMover.Moved -= OnMove;
    }

    private void HandleHit(Sides hitSide)
    {
        if (hitSide == Sides.Left)
        {
            Debug.Log("Left");
        }
        else if (hitSide == Sides.Right)
        {
            Debug.Log("right");
        }
        else if (hitSide == Sides.Up)
        {
            Debug.Log("up");
        }
        else if (hitSide == Sides.Down)
        {
            Debug.Log("Down");
        }
    }

    private void OnMove()
    {
        _moveSound.PlayOneShot(_moveSound.clip);
    }
}