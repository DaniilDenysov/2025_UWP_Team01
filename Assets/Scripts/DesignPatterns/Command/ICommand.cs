using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICommand
{
    bool Execute();
    void Undo();
}
