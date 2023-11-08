using UnityEngine;

public class CharacterOutline : MonoBehaviour
{
    [SerializeField] private float _thickness;
    [SerializeField] private Material _material;

    private const float MinThickness = 0f;
    
    private readonly int _outlineThickness = Shader.PropertyToID("_OutlineThickness");

    private void Start()
    {
        Hide();
    }

    public void Show()
    {
        _material.SetFloat(_outlineThickness, _thickness);
    }

    public void Hide()
    {
        _material.SetFloat(_outlineThickness, MinThickness);
    }
}