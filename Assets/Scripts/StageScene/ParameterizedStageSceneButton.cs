using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public abstract class ParameterizedStageSceneButton<T> : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] protected UnityEvent<T> onClick;

    public abstract void OnPointerClick(PointerEventData eventData);
}
