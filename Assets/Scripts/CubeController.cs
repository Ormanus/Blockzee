using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeController : MonoBehaviour
{
    public static bool CubeAnimating = false;
    public static bool BlockAnimating = false;
    public static bool SceneTransition = false;

    public enum Direction
    {
        Right,
        Up,
        Left,
        Down
    }

    public Cube[] Cubes;

    Cube Selected;

    private void Start()
    {
        if (Cubes.Length > 0)
            Selected = Cubes[0];
        else
        {
            Debug.LogError("No cubes in the list!");
        }
    }

    bool AllowInput
    {
        get { return !CubeAnimating && !SceneTransition && !BlockAnimating; }
    }

    public void Move(Direction direction)
    {
        if (AllowInput)
            Selected.Move(direction);
    }
}
