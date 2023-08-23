using System;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public event Action Closed;

    private void OnDisable()
    { 
        Closed?.Invoke();
    }
}
