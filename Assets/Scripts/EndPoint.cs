using System;
using UnityEngine;

public class EndPoint : MonoBehaviour
{
    public event Action Triggered;
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Character character))
        {
            Triggered?.Invoke();
        }
    }
}
