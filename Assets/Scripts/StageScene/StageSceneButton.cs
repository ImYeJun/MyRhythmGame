using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class StageSceneButton : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] protected UnityEvent onClick;

    public virtual void OnPointerClick(PointerEventData eventData) { onClick.Invoke(); }
}
