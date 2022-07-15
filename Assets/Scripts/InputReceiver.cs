using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputReceiver : MonoBehaviour
{
    public CubeController cubeController;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            cubeController.Move(CubeController.Direction.Right);
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            cubeController.Move(CubeController.Direction.Up);
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            cubeController.Move(CubeController.Direction.Left);
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            cubeController.Move(CubeController.Direction.Down);
        }
    }
}
