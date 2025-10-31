using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class MainMenuButton : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
{
    private TextMeshProUGUI text;
    [SerializeField] private Color defaultTextColor;
    [SerializeField] private Color selectedTextColor;
    [SerializeField] private GameSceneManager.SceneType sceneType;

    public GameSceneManager.SceneType SceneType { get => sceneType; }

    public event Action onHover;
    public event Action onClick;

    private void Awake() {
        text = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void ActivateSelectedEffect()
    {
        text.color = selectedTextColor;
    }

    public void SetDefaultState()
    {
        text.color = defaultTextColor;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        onHover?.Invoke();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        onClick?.Invoke();
    }

    public void SetEventNull()
    {
        onHover = null;
        onClick = null;
    }
}
