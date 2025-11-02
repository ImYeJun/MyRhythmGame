using UnityEngine;

public class StageSceneInputHandler : MonoBehaviour, IInputHandler
{
    [SerializeField] private TrackOptionController trackOptionController;
    [SerializeField] private TrackListPanel trackListPanel;
    [SerializeField] private SelectedTrackDisplay selectedTrackDisplay;

    private void Start()
    {
        InputManager.Instance.AddInputHandler(InputManager.InputPriority.MainUI, this);
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

        //TODO 입력 키에 따라서 trackOptionController,trackListPanel,selectedTrackDisplay의 메소드 호출하기
    }
}
