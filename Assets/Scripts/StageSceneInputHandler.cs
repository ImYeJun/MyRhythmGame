using UnityEngine;

public class StageSceneInputHandler : MonoBehaviour, IInputHandler
{
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
    }
}
