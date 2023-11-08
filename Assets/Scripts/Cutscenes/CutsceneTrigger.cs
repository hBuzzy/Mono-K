using System;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]

public class CutsceneTrigger : Trigger
{
    public override event Action Triggered;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out Character character))
        {
            Triggered?.Invoke();
            gameObject.SetActive(false);
        }
    }
}