using System;
using UnityEngine;

public abstract class Trigger : MonoBehaviour
{
    public abstract event Action Triggered;
}