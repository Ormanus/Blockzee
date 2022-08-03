using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct UndoableAction
{
    public int[] data;
    public int move;
}

public abstract class Undoable : Block
{
    protected Stack<UndoableAction> _actions = new Stack<UndoableAction>();

    public abstract void Do(int move);
    public abstract void Undo(int move);
}
