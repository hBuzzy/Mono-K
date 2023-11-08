using System;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]

public class SoundZone : MonoBehaviour
{
    public event Action<bool> CharacterInsideChanged;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out Character character))
        {
            CharacterInsideChanged?.Invoke(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.TryGetComponent(out Character character))
        {
            CharacterInsideChanged?.Invoke(false);
        }
    }
}