using System;
using UnityEngine;

public class InputManager : GameManager<InputManager>
{
    public event Action inputEvents;

    private void Update() {
        if (!Input.anyKey) return;

        inputEvents?.Invoke();
    }
}
