using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FadeController : MonoBehaviour
{
    public float fadeDuration = 0.5f;
    public static FadeController Instance;
    public FadeController()
    {
        Instance = this;
    }

    float endFadeStartTime;
    bool endFading;

    string targetScene;

    Image image;

    private void Awake()
    {
        image = GetComponent<Image>();
        image.color = new Color(0, 0, 0, 1f);
    }

    public void EndFade(Object scene)
    {
        endFading = true;
        endFadeStartTime = Time.time;

        targetScene = scene.name;
    }

    // Update is called once per frame
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
