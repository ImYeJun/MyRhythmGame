using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSceneManager : GameManager<GameSceneManager>
{
    public enum SceneType { TestScene, MusicSelectScene, MainMenuScene }
    
    public void LoadScene(SceneType type)
    {
        SceneManager.LoadScene((int)type);
    }
}
