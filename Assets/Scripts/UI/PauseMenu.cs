using System;
using UnityEngine;

public class PauseMenu : MonoBehaviour, IClosable
{
    public event Action Closed;

    private void OnDisable()
    { 
        Closed?.Invoke();
    }
}
