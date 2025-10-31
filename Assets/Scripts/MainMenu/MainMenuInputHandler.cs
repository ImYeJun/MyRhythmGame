using System;
using UnityEngine;

public class MainMenuInputHandler : MonoBehaviour, IInputHandler
{
    [SerializeField] private MainMenuButton[] mainMenuButtons;

    private const int INITIAL_BUTTON_INDEX = 1;
    private int currentButtonIndex = 0;

    [SerializeField] private AudioClip buttonSelectedSFX;
    [SerializeField] private AudioClip sceneEnterSFX;

    private void Start()
    {
        for (int i = 0; i < mainMenuButtons.Length; i++)
        {
            int index = i;
            mainMenuButtons[i].onHover += () => { SetCurrentButtonIndex(index); };
            mainMenuButtons[i].onClick += () => {
                if (currentButtonIndex != index) { SetCurrentButtonIndex(index, false); }
                LoadSelctedScene();
            };
        }

        InputManager.Instance.AddInputHandler(InputManager.InputPriority.MainUI, this);

        SetCurrentButtonIndex(INITIAL_BUTTON_INDEX, false);
    }

    private void OnDestroy()
    {
        InputManager.Instance.RemoveInputHandler(this);

        foreach (MainMenuButton mainMenuButton in mainMenuButtons)
        {
            mainMenuButton.SetEventNull();
        }
    }

    public void ProcessInput()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            SetCurrentButtonIndex(currentButtonIndex + 1);
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            SetCurrentButtonIndex(currentButtonIndex - 1);
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            LoadSelctedScene();
        }
    }

    private void LoadSelctedScene()
    {
        SoundManager.Instance.PlaySoundEffect(sceneEnterSFX);
        Debug.Log($"Change Scene To {mainMenuButtons[currentButtonIndex].SceneType}");
    }

    private void SetCurrentButtonIndex(int value, bool playSFX = true)
    {
        if (value >= 0 && value <= (mainMenuButtons.Length - 1) && value != currentButtonIndex)
        {
            mainMenuButtons[currentButtonIndex].SetDefaultState();
            currentButtonIndex = value;
            mainMenuButtons[currentButtonIndex].ActivateSelectedEffect();

            if (playSFX)
            {
                SoundManager.Instance.PlaySoundEffect(buttonSelectedSFX);
            }
        }
    }
}
