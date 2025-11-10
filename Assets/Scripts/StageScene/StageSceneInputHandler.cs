using System;
using System.Collections;
using UnityEditor;
using UnityEngine;

public class StageSceneInputHandler : MonoBehaviour, IInputHandler
{
    [SerializeField] private TrackOptionController trackOptionController;
    [SerializeField] private TrackListPanel trackListPanel;
    [SerializeField] private SelectedTrackDisplay selectedTrackDisplay;

    private void Start()
    {
        InputManager.Instance.AddInputHandler(InputManager.InputPriority.MainUI, this);
        StartCoroutine(WaitTrackListPanelReloaded());
    }

    private IEnumerator WaitTrackListPanelReloaded()
    {
        yield return new WaitUntil(() => trackListPanel.HasReloaded);

        SyncSelectedTrackToDisplay();
    }

    private void OnDestroy()
    {
        InputManager.Instance.RemoveInputHandler(this);
    }

    public void ProcessInput()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GameSceneManager.Instance.LoadScene(GameSceneManager.SceneType.MainMenuScene);
        }
        
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            trackListPanel.MovePreviousSelection();
            SyncSelectedTrackToDisplay();
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            trackListPanel.MoveNextSelection();
            SyncSelectedTrackToDisplay();
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            trackOptionController.DowngradeJudgeLevel();
            SyncSelectedJudgeToDisplay();
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            trackOptionController.UpgradeJudgeLevel();
            SyncSelectedJudgeToDisplay();
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            trackOptionController.DecreaseNoteSpeed();
            SyncSelectedJudgeToDisplay();
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            trackOptionController.IncreaseNoteSpeed();
            SyncSelectedJudgeToDisplay();
        }

        if (Input.GetKeyDown(KeyCode.Delete))
        {
            DeletedSelectedTrack();
        }
        if (Input.GetKeyDown(KeyCode.Insert))
        {
            InsertTrack();
        }
    }

    private void SyncSelectedJudgeToDisplay()
    {
        selectedTrackDisplay.SetJudgeLevel(trackOptionController.CurrentJudgeLevel);
    }

    private void SyncSelectedTrackToDisplay()
    {
        TrackMap currentTrack = trackListPanel.SelectedTrack;
        selectedTrackDisplay.SetSelectedTrack(currentTrack, trackOptionController.CurrentJudgeLevel);
    }

    [ContextMenu("InsertTrack")]
    private void InsertTrack()
    {
        if (TrackMapManager.Instance.TryInsertTrack())
        {
            trackListPanel.ReloadPanel();
            SyncSelectedTrackToDisplay();
            SyncSelectedJudgeToDisplay();
        }
    }

    private void DeletedSelectedTrack()
    {
        if (TrackMapManager.Instance.TryDeleteTrack(trackListPanel.SelectedTrack))
        {
            trackListPanel.ReloadPanel();
            SyncSelectedTrackToDisplay();
            SyncSelectedJudgeToDisplay();
        }
    }
}
