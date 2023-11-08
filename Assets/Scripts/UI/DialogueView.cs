using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueView : View<DialogueLine>
{
    [Header("Model component")]
    [SerializeField] private Dialogue _dialogue;

    [Header("View components")] 
    [SerializeField] private Image _dialogueWindow;
    [SerializeField] private Image _speakerImage;
    [SerializeField] private TMP_Text _speakerName;
    [SerializeField] private TMP_Text _dialogueText;

    private Coroutine _typingCoroutine;
    private float _renderingTime;

    private void OnEnable()
    {
        _dialogue.RenderRequired += OnRenderRequired;
    }

    private void OnDisable()
    {
        _dialogue.RenderRequired -= OnRenderRequired;
    }

    protected override void Render(DialogueLine dialogue)
    {
        SetView(dialogue);

        if (_typingCoroutine != null)
            StopCoroutine(_typingCoroutine);

        _typingCoroutine = StartCoroutine(RenderText(dialogue.Text));
    }

    private IEnumerator RenderText(string text)
    {
        var wait = new WaitForSeconds(_renderingTime);
        
        foreach (char symbol in text)
        {
            _dialogueText.text += symbol;
            yield return wait;
        }

        _typingCoroutine = null;
    }

    private void SetView(DialogueLine dialogue)
    {
        _dialogueWindow.color = dialogue.SpeakerProfile.BackgroundColor;
        _speakerImage.sprite = dialogue.SpeakerProfile.Icon;
        _speakerName.text = dialogue.SpeakerProfile.Name;
        _dialogueText.color = dialogue.SpeakerProfile.FontColor;
        _dialogueText.text = String.Empty;
    }

    private void OnRenderRequired(DialogueLine dialogue, float renderingTime)
    {
        _renderingTime = renderingTime;

        Render(dialogue);
    }
}