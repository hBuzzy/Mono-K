using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonEffects : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler
{
    [SerializeField] private ButtonSounds _buttonSounds;
    
    public void OnPointerDown(PointerEventData eventData)
    {
        _buttonSounds.PlayClickSound();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _buttonSounds.PlayHoverSound();
    }
}