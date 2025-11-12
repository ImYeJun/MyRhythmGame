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
        if (Input.GetKeyDown(KeyCode.Escape)) { ReturnToMainMenuScene(); } 

        if (Input.GetKeyDown(KeyCode.UpArrow)) { MovePreviousSelection(); } 
        if (Input.GetKeyDown(KeyCode.DownArrow)) { MoveNextSelection(); } 

        if (Input.GetKeyDown(KeyCode.LeftArrow)) { DowngradeJudgeLevel(); } 
        if (Input.GetKeyDown(KeyCode.RightArrow)) { UpgradeJudgeLevel(); } 

        if (Input.GetKeyDown(KeyCode.Alpha1)) { DecreaseNoteSpeed(); } 
        if (Input.GetKeyDown(KeyCode.Alpha2)) { IncreaseNoteSpeed(); } 

        if (Input.GetKeyDown(KeyCode.Delete)) { DeletedSelectedTrack(); } 
        if (Input.GetKeyDown(KeyCode.Insert)) { InsertTrack(); } 

        if (Input.GetKeyDown(KeyCode.Return)) { EngageTrack(); } 
    }

    public void ReturnToMainMenuScene()
    {
        GameSceneManager.Instance.LoadScene(GameSceneManager.SceneType.MainMenuScene);
    }

    public void MovePreviousSelection()
    {
        trackListPanel.MovePreviousSelection();
        SyncSelectedTrackToDisplay();
    }
    public void MoveNextSelection()
    {
        trackListPanel.MoveNextSelection();
        SyncSelectedTrackToDisplay();
    }

    public void DowngradeJudgeLevel()
    {
        trackOptionController.DowngradeJudgeLevel();
        SyncSelectedJudgeToDisplay();
    }
    public void UpgradeJudgeLevel()
    {
        trackOptionController.UpgradeJudgeLevel();
        SyncSelectedJudgeToDisplay();
    }
    public void SetJudgeLevel(JudgeLevel judgeLevel) {
        trackOptionController.CurrentJudgeLevel = judgeLevel;
        SyncSelectedJudgeToDisplay();
    }
    
    public void DecreaseNoteSpeed()
    {
        trackOptionController.DecreaseNoteSpeed();
        SyncSelectedJudgeToDisplay();
    }
    public void IncreaseNoteSpeed()
    {
        trackOptionController.IncreaseNoteSpeed();
        SyncSelectedJudgeToDisplay();
    }

    public void DeletedSelectedTrack()
    {
        if (TrackMapManager.Instance.TryDeleteTrack(trackListPanel.SelectedTrack))
        {
            trackListPanel.ReloadPanel();
            SyncSelectedTrackToDisplay();
            SyncSelectedJudgeToDisplay();
        }
    }
    public void InsertTrack()
    {
        Debug.Log(Application.persistentDataPath);
        if (TrackMapManager.Instance.TryLoadTrackMap())
        {
            trackListPanel.ReloadPanel();
            SyncSelectedTrackToDisplay();
            SyncSelectedJudgeToDisplay();
        }
    }

    public void EngageTrack()
    {
        if (trackListPanel.SelectedTrack is not null)
        {
            JudgeLevel selectedJudgeLevel = trackOptionController.CurrentJudgeLevel;
            float selectedNoteSpeed = trackOptionController.CurrentNoteSpeed;

            //* This code will be replaced with the Engage System call implementation later.
            Debug.Log($"Track Title : {trackListPanel.SelectedTrack.TrackTitle}, Judge Level : {selectedJudgeLevel}, Note Speed : {selectedNoteSpeed}");
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
}
