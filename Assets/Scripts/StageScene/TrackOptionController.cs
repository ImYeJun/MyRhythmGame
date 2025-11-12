using System;
using TMPro;
using UnityEngine;

public class TrackOptionController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI noteSpeedText;

    public const float MIN_NOTE_SPEED = 1.0f;
    public const float MAX_NOTE_SPEED = 10.0f;
    private const float NOTE_SPEED_STEP = 0.1f;
    private const float EPSILON = 0.0001f;

    [Header("Default Values")]
    [SerializeField] private float DEFAULT_NOTE_SPEED;
    [SerializeField] private JudgeLevel DEFAULT_JUDGE_LEVEL;

    [Header("Actual Values")]
    private float currentNoteSpeed;
    private JudgeLevel currentJudgeLevel;

    public float CurrentNoteSpeed { get => currentNoteSpeed;
            set
            {
                if (value >= MIN_NOTE_SPEED - EPSILON && value <= MAX_NOTE_SPEED + EPSILON)
                {
                    currentNoteSpeed = (float)(Math.Round(value * 10) / 10);
                    syncNoteSpeedUI();
                }
            }
        }

    private void syncNoteSpeedUI()
    {
        noteSpeedText.text = currentNoteSpeed.ToString();
    }

    public JudgeLevel CurrentJudgeLevel { get => currentJudgeLevel; set => currentJudgeLevel = value; }

    private void Awake()
    {
        CurrentNoteSpeed = DEFAULT_NOTE_SPEED;
        CurrentJudgeLevel = DEFAULT_JUDGE_LEVEL;
    }

    public void IncreaseNoteSpeed() { CurrentNoteSpeed += NOTE_SPEED_STEP; }
    public void DecreaseNoteSpeed() { CurrentNoteSpeed -= NOTE_SPEED_STEP; }

    public void UpgradeJudgeLevel()
    {
        switch (currentJudgeLevel)
        {
            case JudgeLevel.Basic:
                CurrentJudgeLevel = JudgeLevel.Intermediate;
                break;
            case JudgeLevel.Intermediate:
                CurrentJudgeLevel = JudgeLevel.Professional;
                break;
        }
    }

    public void DowngradeJudgeLevel()
    {
        switch (currentJudgeLevel)
        {
            case JudgeLevel.Intermediate:
                CurrentJudgeLevel = JudgeLevel.Basic;
                break;
            case JudgeLevel.Professional:
                CurrentJudgeLevel = JudgeLevel.Intermediate;
                break;
        }
    }
}

public enum JudgeLevel { Basic, Intermediate, Professional }
