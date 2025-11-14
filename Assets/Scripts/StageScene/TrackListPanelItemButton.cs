using UnityEngine;
using UnityEngine.EventSystems;

public class TrackListPanelItemButton : ParameterizedStageSceneButton<int>
{
    [SerializeField] private int index;

    public int Index { get => index; set => index = value; }
    public override void OnPointerClick(PointerEventData eventData) { OnClick.Invoke(index); }
}
