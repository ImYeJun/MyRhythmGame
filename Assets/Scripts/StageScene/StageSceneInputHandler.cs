using System;
using System.Collections;
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
        
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            trackOptionController.DecreaseNoteSpeed();
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            trackOptionController.IncreaseNoteSpeed();
        }
    }

    private void SyncSelectedTrackToDisplay()
    {
        TrackMap currentTrack = trackListPanel.SelectedTrack();
        selectedTrackDisplay.SetSelectedTrack(currentTrack, trackOptionController.CurrentJudgeLevel);
    }
}
