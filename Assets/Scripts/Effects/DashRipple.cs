using UnityEngine;
using UnityEngine.VFX;

[RequireComponent(typeof(VisualEffect))]

public class DashRipple : MonoBehaviour
{
    private const float AdditionalHeight = 0.5f;
    private VisualEffect _ripple;

    private void Start()
    {
        _ripple = GetComponent<VisualEffect>();
    }

    public void Play(Vector2 position)
    {
        transform.position = new Vector3(position.x, position.y + AdditionalHeight);
        
        _ripple.Stop();
        _ripple.Play();
    }
}