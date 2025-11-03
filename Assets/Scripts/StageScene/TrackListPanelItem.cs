using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TrackListPanelItem : MonoBehaviour
{
    [SerializeField] private Image background;
    [SerializeField] private Image trackCoverImage;
    [SerializeField] private TextMeshProUGUI trackTitleText;
    [SerializeField] private TextMeshProUGUI composerText;
    [SerializeField] private TextMeshProUGUI bpmText;
    [SerializeField] private TextMeshProUGUI basicResultText;
    [SerializeField] private TextMeshProUGUI intermediateResultText;
    [SerializeField] private TextMeshProUGUI professionalResultText;
    private BeatMap beatMap;
    public BeatMap BeatMap { get => beatMap; }

    public void Init(BeatMap beatMap)
    {
        this.beatMap = beatMap;
        
        trackCoverImage.sprite = beatMap.CoverImage;
        trackTitleText.text = beatMap.TrackTitle;
        composerText.text = beatMap.Composer;

        float minBpm = beatMap.MinBpm, maxBpm = beatMap.MaxBpm;
        string bpmTextContent;
        if (minBpm == maxBpm) { bpmTextContent = minBpm.ToString(); }
        else { bpmTextContent = $"{minBpm} ~ {maxBpm}"; }
        bpmText.text = bpmTextContent;

        RefreshJudgeResultEffect();
        SetDefault();
    }

    public void RefreshJudgeResultEffect()
    {
        //TODO 플레이 결과값 UI 설정 보여주기
    }

    public void SetDefault()
    {
        background.color = Color.white;
    }

    public void SetSelected()
    {
        background.color = Color.magenta;
    }
}
