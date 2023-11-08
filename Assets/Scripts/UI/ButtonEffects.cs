using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonEffects : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler
{
    [SerializeField] private ButtonSounds _sounds;
    
    public void OnPointerDown(PointerEventData eventData)
    {
        UISounds.Instance.PlaySound(_sounds.GetClickSound());
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        UISounds.Instance.PlaySound(_sounds.GetHoverSound());
    }
}