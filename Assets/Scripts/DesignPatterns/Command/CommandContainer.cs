using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class CommandContainer : MonoBehaviour
{
    private Stack<ICommand> undoStack = new Stack<ICommand>();
    private Stack<ICommand> redoStack = new Stack<ICommand>();

    public void ExecuteCommand(ICommand command)
    {
        if (command.Execute())
        {
            undoStack.Push(command);
            redoStack.Clear();
        }
    }

    public void Undo()
    {
        var command = undoStack.Pop();
        command.Undo();
        redoStack.Push(command);
    }

    public void Redo()
    {
        redoStack.Pop().Execute();
    }
}
