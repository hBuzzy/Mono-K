using UnityEngine;

[CreateAssetMenu(menuName = "Dialogues/New Dialogue Profile")]

public class DialogueProfile : ScriptableObject
{
    [SerializeField] private Sprite _icon;
    [SerializeField] private string _name;
    [SerializeField] private Color _backgroundColor;
    [SerializeField] private Color _fontColor;

    public Sprite Icon => _icon;
    public string Name => _name;
    public Color BackgroundColor => _backgroundColor;
    public Color FontColor => _fontColor;
}