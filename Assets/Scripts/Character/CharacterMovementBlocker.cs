using System;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class CharacterMovementBlocker : MonoBehaviour
{
    public static CharacterMovementBlocker Instance;

    private bool _canMove = true;

    public bool CanMove => _canMove;
    
    private void OnEnable()
    {
        Instance = this;
    }

    public void DisableMovement()
    {
        _canMove = false;
    }

    public void EnableMovement()
    {
        _canMove = true;
    }
}