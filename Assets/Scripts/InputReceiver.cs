using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputReceiver : MonoBehaviour
{
    public CubeController cubeController;

    public void OnMovementInput(InputAction.CallbackContext context)
    {
        var value = context.ReadValue<Vector2>();

        if (value != null)
        {
            if (value.x > 0.1f)
            {
                cubeController.Move(CubeController.Direction.Right);
            }
            if (value.x < -0.1f)
            {
                cubeController.Move(CubeController.Direction.Left);
            }
            if (value.y > 0.1f)
            {
                cubeController.Move(CubeController.Direction.Up);
            }
            if (value.y < -0.1f)
            {
                cubeController.Move(CubeController.Direction.Down);
            }
        }
    }

    public void CubeSelector(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            float dir = context.ReadValue<float>();
            if (dir > 0.1f)
            {
                cubeController.NextCube();
            }
            if (dir < -0.1f)
            {
                cubeController.PreviousCube();
            }
        }
    }

    public void CubeSelectnumber(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            cubeController.SelectCube(int.Parse(context.control.name) - 1);
        }
    }
}
