using UnityEngine;

public class Hurt : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.TryGetComponent(out CharacterHurt character))
        {
            character.OnHurt();
        }
    }
}