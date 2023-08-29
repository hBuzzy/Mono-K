using UnityEngine;

public abstract class View<T> : MonoBehaviour
{
    protected abstract void Render(T value);
}