using System;
using System.Collections;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    [SerializeField] private AudioSource _explosionSound;
    [SerializeField] private float _respawnAfterTime;
    
    private readonly int _explosion = Animator.StringToHash("Explosion");

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Character character))
        {
            
        }
    }

    private IEnumerator Explosion()
    {
        yield return null;
    }
}
