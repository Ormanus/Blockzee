using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MainMenuController : MonoBehaviour
{
    public LevelButton[] row1;
    public LevelButton[] row2;

    Vector2Int selection = new Vector2Int(0, 0);

    float lastMovement = 0f;
    const float movementDelay = 0.3f;


    private void Start()
    {
        GetButton().SetSelected(true);
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

        GetButton().SetSelected(false);

        if (Mathf.Abs(value.x) > Mathf.Abs(value.y))
        {
            if (value.x > 0.0f)
            {
                selection = new Vector2Int((selection.x + 1) % 4, selection.y);
            }
            else if (value.x < 0.0f)
            {
                selection = new Vector2Int((selection.x + 3) % 4, selection.y);
            }
        }
        else
        {
            if (value.y > 0.0f)
            {
                selection = new Vector2Int(selection.x, (selection.y + 1) % 2);
            }
            else if (value.y < 0.0f)
            {
                selection = new Vector2Int(selection.x, (selection.y + 1) % 2);
            }
        }

        lastMovement = Time.time;
        GetButton().SetSelected(true);
        AudioManager.PlayClip(AudioManager.Instance.ClickSound);
    }

    LevelButton GetButton()
    {
        return selection.y switch
        {
            0 => row1[selection.x],
            1 => row2[selection.x],
            _ => null,
        };
    }

    public void Select(InputAction.CallbackContext context)
    {
        if (context.phase != InputActionPhase.Started)
            return;

        GetButton().StartLevel();
    }

    public void Quit()
    {
        Application.Quit();
    }
}
