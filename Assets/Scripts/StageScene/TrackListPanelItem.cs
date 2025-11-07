using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TrackListPanelItem : MonoBehaviour
{
    private static readonly Color SELECTED_COLOR;
    private static readonly Color UNSELECTED_COLOR;
    private static readonly Color UNSTAGED_COLOR;
    private static readonly Color STAGED_COLOR;
    private static readonly Color MASTERED_COLOR;
    private static readonly Color EUPHORIC_COLOR;
    static TrackListPanelItem()
    {
        ColorUtility.TryParseHtmlString("#e66815ff", out SELECTED_COLOR);
        ColorUtility.TryParseHtmlString("#707070ff", out UNSELECTED_COLOR);
        ColorUtility.TryParseHtmlString("#3A3A3A", out UNSTAGED_COLOR);
        ColorUtility.TryParseHtmlString("#5EF38C", out STAGED_COLOR);
        ColorUtility.TryParseHtmlString("#FFD700", out MASTERED_COLOR);
        ColorUtility.TryParseHtmlString("#4eddddff", out EUPHORIC_COLOR);
    }

    [SerializeField] private Image background;
    [SerializeField] private Image trackCoverImage;
    [SerializeField] private TextMeshProUGUI trackTitleText;
    [SerializeField] private TextMeshProUGUI composerText;
    [SerializeField] private TextMeshProUGUI bpmText;
    [SerializeField] private TextMeshProUGUI basicResultText;
    [SerializeField] private TextMeshProUGUI intermediateResultText;
    [SerializeField] private TextMeshProUGUI professionalResultText;

    [SerializeField] private List<TextMeshProUGUI> judgeLevelTexts;

    private TrackMap trackMap;
    public TrackMap TrackMap { get => trackMap; }

    public void Init(TrackMap trackMap)
    {
        this.trackMap = trackMap;

        trackCoverImage.sprite = trackMap.CoverImage;
        trackTitleText.text = trackMap.TrackTitle;
        composerText.text = trackMap.Composer;

        float minBpm = trackMap.MinBpm, maxBpm = trackMap.MaxBpm;
        string bpmTextContent;
        if (minBpm == maxBpm) { bpmTextContent = minBpm.ToString(); }
        else { bpmTextContent = $"{minBpm} ~ {maxBpm}"; }
        bpmText.text = bpmTextContent;

        RefreshPlayResultEffect();
        SetDefault();
    }

    public void SetDefault()
    {
        background.color = UNSELECTED_COLOR;
    }

    public void SetSelected()
    {
        background.color = SELECTED_COLOR;
    }

    
    public void RefreshPlayResultEffect()
    {
        PlayResultSet playResultSet;

        if (PlayResultManager.Instance.TryGetPlayResultSet(trackMap.HashCode, out playResultSet))
        {
            foreach (JudgeLevel judgeLevel in Enum.GetValues(typeof(JudgeLevel)))
            {
                PlayResult playResult = playResultSet.GetPlayResult(judgeLevel);
                TextMeshProUGUI judgeLevelText = judgeLevelTexts[(int)judgeLevel];

                if (playResult.isPerfect) { judgeLevelText.color = EUPHORIC_COLOR; }
                else if (playResult.isMaxCombo) { judgeLevelText.color = MASTERED_COLOR; }
                else if (playResult.hasPlayed) { judgeLevelText.color = STAGED_COLOR; }
                else { judgeLevelText.color = UNSTAGED_COLOR; }
            }
        }
        else
        {
            foreach (TextMeshProUGUI judgeLevelText in judgeLevelTexts)
            {
                judgeLevelText.color = UNSTAGED_COLOR;
            }
        }
    }
}
