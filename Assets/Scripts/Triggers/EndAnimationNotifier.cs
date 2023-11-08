using System;
using UnityEngine;

public class EndAnimationNotifier : MonoBehaviour
{
    public event Action Ended;

    public void OnAnimationEnded()
    {
        Ended?.Invoke();
    }
}