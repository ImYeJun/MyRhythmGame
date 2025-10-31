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
        internal IInputHanlder inputHanlder;

        public InputHandler(int priority, IInputHanlder inputHanlder)
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

    private IInputHanlder GetHighestPriorityInputHandler()
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

    public void AddInputHandler(int priority, IInputHanlder inputHandler)
    {
        inputHandlers.Add(new InputHandler(priority, inputHandler));
        isListDirty = true;
    }

    public void AddInputHandler(InputPriority priority, IInputHanlder inputHanlder)
    {
        AddInputHandler((int)priority, inputHanlder);
    }

    public void AddInputHandler(IInputHanlder inputHandler)
    {
        AddInputHandler(InputPriority.Default, inputHandler);
    }

    public void RemoveInputHandler(IInputHanlder inputHanlder)
    {
        inputHandlers.RemoveAll(x => x.inputHanlder == inputHanlder);
    }
}

public interface IInputHanlder
{
    void ProcessInput();
}