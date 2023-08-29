using System;
using UnityEngine;

public class HitBox : MonoBehaviour
{
    public event Action CharacterEntered;
    
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.TryGetComponent(out CharacterDeath character))
        {
            CharacterEntered?.Invoke();
            character.TakeDamage();
        }
    }
}