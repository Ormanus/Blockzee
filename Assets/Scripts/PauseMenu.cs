using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{
    public PlayerInput Input;
    public GameObject PauseCanvas;

    public Button[] buttons;

    bool paused = false;

    int selection = 0;
    float lastMovement = 0f;
    const float movementDelay = 0.3f;

    private void Start()
    {
        PauseCanvas.SetActive(paused);
        buttons[0].Select();
    }

    public void TogglePause(InputAction.CallbackContext context)
    {
        if (context.phase != InputActionPhase.Started)
            return;

        Debug.Log("Toggle!");

        paused = !paused;
        if (paused)
            Input.currentActionMap = Input.actions.FindActionMap("Menu");
        else
            Input.currentActionMap = Input.actions.FindActionMap("Cube");

        PauseCanvas.SetActive(paused);
    }

    public void Control(InputAction.CallbackContext context)
    {
        if (lastMovement + movementDelay > Time.time)
            return;

        var value = context.ReadValue<Vector2>();

        if (value == null || value.sqrMagnitude < 0.1f)
        {
            return;
        }

        if (value.y > 0.0f)
        {
            selection = (selection + buttons.Length - 1) % buttons.Length;
        }
        else if (value.y < 0.0f)
        {
            selection = (selection + 1) % buttons.Length;
        }

        lastMovement = Time.time;
        buttons[selection].Select();
        AudioManager.PlayClip(AudioManager.Instance.ClickSound);
    }

    public void Enter(InputAction.CallbackContext context)
    {
        if (context.phase != InputActionPhase.Started)
            return;

        buttons[selection].onClick?.Invoke();
    }

    public void MainMenu()
    {
        FadeController.Instance.EndFade("MainMenu");
    }

    public void Restart()
    {
        CubeController.Instance.Restart();
    }
}
