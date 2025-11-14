using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public abstract class ParameterizedStageSceneButton<T> : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] protected UnityEvent<T> onClick;

    public UnityEvent<T> OnClick { get => onClick; set => onClick = value; }
    public abstract void OnPointerClick(PointerEventData eventData);
}
