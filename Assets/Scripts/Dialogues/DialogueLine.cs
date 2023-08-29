using UnityEngine;

[CreateAssetMenu(menuName = "Dialogues/New Dialogue Line")]

public class DialogueLine : ScriptableObject
{
    [SerializeField] private DialogueProfile _speakerProfile;
    [SerializeField] [TextArea(5, 10)] private string _text;

    public DialogueProfile SpeakerProfile => _speakerProfile;
    public string Text => _text;
}