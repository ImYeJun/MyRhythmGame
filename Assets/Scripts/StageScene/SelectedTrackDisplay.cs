using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SelectedTrackDisplay : MonoBehaviour
{
    [SerializeField] private Image coverImage;
    [SerializeField] private TextMeshProUGUI trackTitleText;
    [SerializeField] private TextMeshProUGUI composerText;
    [SerializeField] private TextMeshProUGUI bpmText;
    private BeatMap selectedTrack;

    private void Awake()
    {
        SetDefault();
    }

    public void SetDefault()
    {
        selectedTrack = null;

        coverImage.sprite = null;
        trackTitleText.text = "None";
        composerText.text = "None";
        bpmText.text = "Bpm : None";
    }

    public void SetSelectedTrack(BeatMap track, JudgeLevel judgeLevel)
    {
        if (track == null)
        {
            SetDefault();
            return;
        }

        selectedTrack = track;

        coverImage.sprite = selectedTrack.CoverImage;
        trackTitleText.text = selectedTrack.TrackTitle;
        composerText.text = selectedTrack.Composer;

        float minBpm = selectedTrack.MinBpm, maxBpm = selectedTrack.MaxBpm;
        string bpmTextContent = "Bpm : ";
        if (minBpm == maxBpm) { bpmTextContent += minBpm.ToString(); }
        else { bpmTextContent += $"{minBpm} ~ {maxBpm}"; }
        bpmText.text = bpmTextContent;

        SetJudgeLevel(judgeLevel);
    }

    public void SetJudgeLevel(JudgeLevel judgeLevel)
    {
        //TODO GUI 관련 작업을 함
    } 
}

