using UnityEngine;

public class Outline : MonoBehaviour//TODO: Change char material instead of global one
{
    [SerializeField] private float _thickness;
    [SerializeField] private Material _material;
    
    private readonly int _outlineThickness = Shader.PropertyToID("_OutlineThickness");

    public void Show()
    {
        _material.SetFloat(_outlineThickness, _thickness);
    }

    public void Hide()
    {
        _material.SetFloat(_outlineThickness, 0f);
    }
}