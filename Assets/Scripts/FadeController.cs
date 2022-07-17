using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FadeController : MonoBehaviour
{
    public float fadeDuration = 0.5f;
    public static FadeController Instance;

    float endFadeStartTime;
    bool endFading;

    string targetScene;

    Image image;

    private void Awake()
    {
        Instance = this;
        image = GetComponent<Image>();
        image.color = new Color(0, 0, 0, 1f);
    }

    public void EndFade(string sceneName)
    {
        endFading = true;
        endFadeStartTime = Time.time;

        targetScene = sceneName;
    }

    void Update()
    {
        if (endFading)
        {
            float phase = (Time.time - endFadeStartTime) / fadeDuration;
            image.color = new Color(0, 0, 0, phase);
            if (phase >= 1)
            {
                SceneManager.LoadScene(targetScene);
            }
        }
        else
        {
            float phase = Mathf.Max(0, 1 - Time.timeSinceLevelLoad / fadeDuration);
            image.color = new Color(0, 0, 0, phase);
        }
    }
}
