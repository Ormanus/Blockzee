using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImagePulse : MonoBehaviour
{
    public Color Color1;
    public Color Color2;

    Graphic graphic;
    private void Awake()
    {
        graphic = GetComponent<Graphic>();
    }
    void Update()
    {
        graphic.color = Color.Lerp(Color1, Color2, Mathf.Sin(Time.time) * 0.5f + 0.5f);
    }
}
