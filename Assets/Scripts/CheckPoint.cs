using System;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]

public class CheckPoint : MonoBehaviour
{
    [SerializeField] private CharacterRespawner _respawner;
    [SerializeField] private Transform _respawnPoint;

    private bool _isActive;
    
    public Transform RespawnPoint => _respawnPoint;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_isActive)
            return;
        
        if (other.TryGetComponent(out Character character))
        {
            Check();
            _respawner.SetCheckPoint(this);
        }
    }

    public void Uncheck()
    {
        _isActive = false;
    }

    private void Check()
    {
        _isActive = true;
    }
}
