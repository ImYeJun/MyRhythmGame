using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSceneManager : GameManager<GameSceneManager>
{
    public enum SceneType { TestScene, MainMenuScene, OptionScene, StageScene, ComposeScene }
    
    public void LoadScene(SceneType type)
    {
        SceneManager.LoadScene((int)type);
    }
}
