using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MainMenuController : MonoBehaviour
{
    public LevelButton[] row1;
    public LevelButton[] row2;

    Vector2Int selection = new Vector2Int(0, 0);
    private void Start()
    {
        GetButton().SetSelected(true);
    }

    public void Control(InputAction.CallbackContext context)
    {
        if (context.phase != InputActionPhase.Started)
            return;
        
        var value = context.ReadValue<Vector2>();

        if (value == null)
            return;

        GetButton().SetSelected(false);

        if (value.x > 0.1f)
        {
            selection = new Vector2Int((selection.x + 1) % 3, selection.y);
        }
        if (value.x < -0.1f)
        {
            selection = new Vector2Int((selection.x + 2) % 3, selection.y);
        }
        if (value.y > 0.1f)
        {
            selection = new Vector2Int(selection.x, (selection.y + 1) % 2);
        }
        if (value.y < -0.1f)
        {
            selection = new Vector2Int(selection.x, (selection.y + 1) % 2);
        }

        GetButton().SetSelected(true);

        AudioManager.PlayClip(AudioManager.Instance.ClickSound);
    }

    LevelButton GetButton()
    {
        return selection.y == 0 ? row1[selection.x] : row2[selection.x];
    }

    public void Select(InputAction.CallbackContext context)
    {
        if (context.phase != InputActionPhase.Started)
            return;

        GetButton().StartLevel();
    }
}
