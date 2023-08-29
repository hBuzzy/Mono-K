using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dialogue : MonoBehaviour
{
    [SerializeField] private float _startDelay;

    private Queue<DialogueLine> _dialogueLines;

    public event Action<DialogueLine, float> RenderRequired;

    private void Start()
    {
        //_dialogueLines = _dialogueData.GetDialogueQueue();
    }

    private void OnEnable()
    {
        
    }

    public void StartDialogue(DialogueData dialogueData)
    {
        StartCoroutine(RenderDialogue(dialogueData));
    }

    private IEnumerator RenderDialogue(DialogueData dialogueData)
    {
        yield return new WaitForSeconds(_startDelay);

        Queue<DialogueLine> queue = dialogueData.GetDialogueQueue();

        while (queue.Count != 0)
        {
            DialogueLine dialogueLine = queue.Dequeue();
            
            float renderingTime = dialogueLine.Text.Length * dialogueData.TypingSpeed;
            RenderRequired?.Invoke(dialogueLine, dialogueData.TypingSpeed);

            yield return new WaitForSeconds(renderingTime + dialogueData.DelayBetweenLines);
        }
    }
}