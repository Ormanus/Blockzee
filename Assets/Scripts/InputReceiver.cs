using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputReceiver : MonoBehaviour
{
    public static bool CubeAnimating = false;
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
        get { return !CubeAnimating && !SceneTransition; }
    }

    private void Update()
    {
        if (AllowInput)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Application.Quit();
            }

            if (Input.GetKeyDown(KeyCode.D))
            {
                Selected.Move(Direction.Right);
            }

            if (Input.GetKeyDown(KeyCode.W))
            {
                Selected.Move(Direction.Up);
            }

            if (Input.GetKeyDown(KeyCode.A))
            {
                Selected.Move(Direction.Left);
            }

            if (Input.GetKeyDown(KeyCode.S))
            {
                Selected.Move(Direction.Down);
            }


        }
    }
}
