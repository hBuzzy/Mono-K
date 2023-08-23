using System;
using UnityEngine;

public class EndPoint : MonoBehaviour
{
    public event Action Reached;
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Character character))
        {
            Debug.Log("End point reached");
            Reached?.Invoke();
        }
    }
}
