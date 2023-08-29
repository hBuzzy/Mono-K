using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Dialogues/New Dialogue")]

public class DialogueData : ScriptableObject
{
    [Header("Options")]
    [SerializeField] private float _typingSpeed;
    [SerializeField] private float _delayBetweenLines;
    
    [Header("Text")]
    [SerializeField] private DialogueLine[] _dialogueLines;

    public float TypingSpeed => _typingSpeed;
    public float DelayBetweenLines => _delayBetweenLines;

    public Queue<DialogueLine> GetDialogueQueue()
    {
        Queue<DialogueLine> dialogueQueue = new Queue<DialogueLine>();

        foreach (var dialogueLine in _dialogueLines)
        {
            dialogueQueue.Enqueue(dialogueLine);
        }

        return dialogueQueue;
    }
}