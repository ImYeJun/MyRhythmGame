using System;
using System.Collections.Generic;

public class InputManager : GameManager<InputManager>
{
    public enum InputPriority
    {
        MainUI = 0,
        Default = 1000
    }

    private class InputHandler : IComparable<InputHandler>
    {
        internal int priority;
        internal IInputHandler inputHanlder;

        public InputHandler(int priority, IInputHandler inputHanlder)
        {
            this.priority = priority;
            this.inputHanlder = inputHanlder;
        }

        public int CompareTo(InputHandler other)
        {
            return priority.CompareTo(other.priority);
        }
    }

    private List<InputHandler> inputHandlers = new List<InputHandler>();
    private bool isListDirty = true;

    private IInputHandler GetHighestPriorityInputHandler()
    {
        if (inputHandlers.Count == 0) return null;

        if (isListDirty)
        {
            inputHandlers.Sort();
            isListDirty = false;
        }

        return inputHandlers[0].inputHanlder;
    }

    private void Update()
    {
        GetHighestPriorityInputHandler()?.ProcessInput();
    }

    public void AddInputHandler(int priority, IInputHandler inputHandler)
    {
        inputHandlers.Add(new InputHandler(priority, inputHandler));
        isListDirty = true;
    }

    public void AddInputHandler(InputPriority priority, IInputHandler inputHanlder)
    {
        AddInputHandler((int)priority, inputHanlder);
    }

    public void AddInputHandler(IInputHandler inputHandler)
    {
        AddInputHandler(InputPriority.Default, inputHandler);
    }

    public void RemoveInputHandler(IInputHandler inputHanlder)
    {
        inputHandlers.RemoveAll(x => x.inputHanlder == inputHanlder);
    }
}

public interface IInputHandler
{
    void ProcessInput();
}