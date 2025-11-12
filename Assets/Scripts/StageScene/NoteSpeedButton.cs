using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class NoteSpeedButton : StageSceneButton
{
    [SerializeField] private UnityEvent onRightClick;

    public override void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left) { onClick.Invoke(); }
        else if (eventData.button == PointerEventData.InputButton.Right) { onRightClick.Invoke(); }
    }
}
