using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class LevelButton : MonoBehaviour, IPointerDownHandler
{
    public RawImage LevelNumber;
    public Image Completed;
    public Image Yahtzee;
    public Image Selection;
    [Space]
    public int index;
    public string TargetLevel;

    public static bool transitioning = false;

    private void Awake()
    {
        transitioning = false;

        int levelCompletionState = SaveSystem.GetLevelState(index);
        LevelNumber.enabled = false;
        Completed.enabled = false;
        Yahtzee.enabled = false;
        Selection.enabled = false;
        switch (levelCompletionState)
        {
            case 0:
                LevelNumber.enabled = true;
                break;
            case 1:
                Completed.enabled = true;
                break;
            case 2:
                Yahtzee.enabled = true;
                break;
            default:
                break;
        }
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        StartLevel();
    }

    public void StartLevel()
    {
        if (!transitioning)
        {
            transitioning = true;
            AudioManager.PlayClip(AudioManager.Instance.ClickSound);
            Debug.Log("Instance: " + FadeController.Instance);
            Debug.Log("Level name: " + TargetLevel);
            FadeController.Instance.EndFade(TargetLevel);
        }
    }

    public void SetSelected(bool state)
    {
        Selection.enabled = state;
    }
}
