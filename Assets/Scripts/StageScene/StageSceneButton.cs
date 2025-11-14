using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class StageSceneButton : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] protected UnityEvent onClick;

    public UnityEvent OnClick { get => onClick; set => onClick = value; }

    public virtual void OnPointerClick(PointerEventData eventData) { onClick.Invoke(); }
}
