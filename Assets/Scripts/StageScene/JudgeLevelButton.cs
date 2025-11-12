using UnityEngine;
using UnityEngine.EventSystems;

public class JudgeLevelButton : ParameterizedStageSceneButton<JudgeLevel>
{
    [SerializeField] private JudgeLevel judgeLevel;
    
    public override void OnPointerClick(PointerEventData eventData) { onClick.Invoke(judgeLevel); }
}
