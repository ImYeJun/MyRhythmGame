using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SelectedTrackDisplay : MonoBehaviour
{
    private TrackMap selectedTrack;

    [Header("Track Display")]
    [SerializeField] private Image coverImage;
    [SerializeField] private TextMeshProUGUI trackTitleText;
    [SerializeField] private TextMeshProUGUI composerText;
    [SerializeField] private TextMeshProUGUI bpmText;

    [Header("Judge(PlayResult) Display")]
    private const string SCORE_TEXT_PREFIX = "SCORE : ";
    private const string RATE_TEXT_PREFIX = "Rate : ";
    private const string NOT_PLAYED_COMBO_TEXT = "-";
    [SerializeField] private List<TextMeshProUGUI> judgeIndicatorTexts;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI rateText;
    [SerializeField] private TextMeshProUGUI perfectComboText;
    [SerializeField] private TextMeshProUGUI niceComboText;
    [SerializeField] private TextMeshProUGUI goodComboText;
    [SerializeField] private TextMeshProUGUI tlqkfComboText;

    private void Awake()
    {
        SetSelectedTrackDefault();
    }

    public void SetSelectedTrackDefault()
    {
        selectedTrack = null;

        coverImage.sprite = null;
        trackTitleText.text = "None";
        composerText.text = "None";
        bpmText.text = "Bpm : None";

        SetPlayResultDisplayDefault();
    }

    private void SetPlayResultDisplayDefault()
    {
        DisableJudgeIndicatorEffect();

        scoreText.text = SCORE_TEXT_PREFIX;
        rateText.text = RATE_TEXT_PREFIX;
        perfectComboText.text = NOT_PLAYED_COMBO_TEXT;
        niceComboText.text = NOT_PLAYED_COMBO_TEXT;
        goodComboText.text = NOT_PLAYED_COMBO_TEXT;
        tlqkfComboText.text = NOT_PLAYED_COMBO_TEXT;
    }

    public void SetSelectedTrack(TrackMap track, JudgeLevel judgeLevel)
    {
        if (track == null)
        {
            SetSelectedTrackDefault();
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
        if (selectedTrack is null)
        {
            Debug.Log("Track is not selected");
            SetPlayResultDisplayDefault();
            return;
        }

        PlayResult playResult;
        if (!PlayResultManager.Instance.TryGetPlayResult(selectedTrack.HashCode, judgeLevel, out playResult))
        {
            Debug.Log($"There's no Play Result for ({selectedTrack.HashCode}, {judgeLevel})");
            SetPlayResultDisplayDefault();
            return;
        }

        DisableJudgeIndicatorEffect();
        judgeIndicatorTexts[(int)judgeLevel].color = Color.cyan;

        scoreText.text = SCORE_TEXT_PREFIX + playResult.score.ToString();
        rateText.text = RATE_TEXT_PREFIX + playResult.rate.ToString();

        perfectComboText.text = playResult.perfectCount.ToString();
        niceComboText.text = playResult.NiceCount.ToString();
        goodComboText.text = playResult.GoodCount.ToString();
        tlqkfComboText.text = playResult.TlqkfCount.ToString();
    }
    
    private void DisableJudgeIndicatorEffect()
    {
        foreach (TextMeshProUGUI judgeIndicatorText in judgeIndicatorTexts)
        {
            judgeIndicatorText.color = Color.black;
        }
    }

}

