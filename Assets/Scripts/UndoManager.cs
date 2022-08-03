using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UndoManager : MonoBehaviour
{
    public static int move = 0;

    Undoable[] undoables;

    private void Start()
    {
        undoables = FindObjectsOfType<Undoable>(true);
        move = 0;

        // Move cubes to the end to make sure they're processed last
        for (int i = 0; i < undoables.Length - 5; i++)
        {
            if (undoables[i] is Cube)
            {
                for (int j = undoables.Length - 1; j >= 0; j--)
                {
                    if (undoables[j] is not Cube)
                    {
                        (undoables[j], undoables[i]) = (undoables[i], undoables[j]);
                        break;
                    }
                }
            }
        }
    }

    public void Do()
    {
        foreach (Undoable undoable in undoables)
        {
            undoable.Do(move);
        }
        move++;
    }

    public void Undo(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            Undo();
        }
    }

    public void Undo()
    {
        move--;
        foreach (Undoable undoable in undoables)
        {
            undoable.Undo(move);
        }
    }
}
